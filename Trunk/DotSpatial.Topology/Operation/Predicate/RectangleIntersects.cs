// ********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System.Collections.Generic;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Algorithm.Locate;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Geometries.Utilities;

namespace DotSpatial.Topology.Operation.Predicate
{
    /// <summary>Implementation of the <tt>Intersects</tt> spatial predicate
    /// optimized for the case where one <see cref="IGeometry"/> is a rectangle. 
    /// </summary>
    /// <remarks>
    /// This class works for all input geometries, including <see cref="IGeometryCollection"/>s.
    /// <para/>
    /// As a further optimization, this class can be used in batch style
    /// to test many geometries against a single rectangle.
    /// </remarks>
    public class RectangleIntersects
    {
        #region Constant Fields

        /// <summary>
        /// Crossover size at which brute-force intersection scanning
        /// is slower than indexed intersection detection.
        /// Must be determined empirically.  Should err on the
        /// safe side by making value smaller rather than larger.
        /// </summary>
        public const int MaximumScanSegmentCount = 200;

        #endregion

        #region Fields

        private readonly IPolygon _rectangle;
        private readonly IEnvelope _rectEnv;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new intersects computer for a rectangle.
        /// </summary>
        /// <param name="rectangle">A rectangular polygon.</param>
        public RectangleIntersects(IPolygon rectangle)
        {
            _rectangle = rectangle;
            _rectEnv = rectangle.EnvelopeInternal;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tests whether a rectangle intersects a given geometry.
        /// </summary>
        /// <param name="rectangle">A rectangular polygon</param>
        /// <param name="b">A geometry of any kind</param>
        /// <returns><c>true</c> if the geometries intersect.</returns>
        public static bool Intersects(IPolygon rectangle, IGeometry b)
        {
            var rp = new RectangleIntersects(rectangle);
            return rp.Intersects(b);
        }

        /// <summary>
        /// Tests whether the given Geometry intersects the query rectangle.
        /// </summary>
        /// <param name="geom">The Geometry to test (may be of any type)</param>
        /// <returns><value>true</value> if an intersection must occur 
        /// or <value>false</value> if no conclusion about intersection can be made</returns>
        public bool Intersects(IGeometry geom)
        {
            if (!_rectEnv.Intersects(geom.EnvelopeInternal))
                return false;

            /**
             * Test if rectangle envelope intersects any component envelope.
             * This handles Point components as well
             */
            var visitor = new EnvelopeIntersectsVisitor(_rectEnv);
            visitor.ApplyTo(geom);
            if (visitor.Intersects)
                return true;

            /**
             * Test if any rectangle vertex is contained in the target geometry
             */
            var ecpVisitor = new GeometryContainsPointVisitor(_rectangle);
            ecpVisitor.ApplyTo(geom);
            if (ecpVisitor.ContainsPoint)
                return true;

            /**
             * Test if any target geometry line segment intersects the rectangle
             */
            var riVisitor = new RectangleIntersectsSegmentVisitor(_rectangle);
            riVisitor.ApplyTo(geom);
            return riVisitor.Intersects;
        }

        #endregion
    }

    /// <summary>
    /// Tests whether it can be concluded that a rectangle intersects a geometry,
    /// based on the relationship of the envelope(s) of the geometry.
    /// </summary>
    /// <author>Martin Davis</author>
    internal class EnvelopeIntersectsVisitor : ShortCircuitedGeometryVisitor
    {
        #region Fields

        private readonly IEnvelope _rectEnv;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of this class using the provided <c>Envelope</c>
        /// </summary>
        /// <param name="rectEnv">The query envelope</param>
        public EnvelopeIntersectsVisitor(IEnvelope rectEnv)
        {
            _rectEnv = rectEnv;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Reports whether it can be concluded that an intersection occurs, 
        /// or whether further testing is required.
        /// </summary>
        /// <returns><c>true</c> if an intersection must occur <br/> 
        /// or <c>false</c> if no conclusion about intersection can be made</returns>
        public bool Intersects { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool IsDone()
        {
            return Intersects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        protected override void Visit(IGeometry element)
        {
            var elementEnv = element.EnvelopeInternal;

            // disjoint => no intersection
            if (!_rectEnv.Intersects(elementEnv))
                return;

            // rectangle contains target env => must intersect
            if (_rectEnv.Contains(elementEnv))
            {
                Intersects = true;
                return;
            }
            /*
             * Since the envelopes intersect and the test element is connected,
             * if its envelope is completely bisected by an edge of the rectangle
             * the element and the rectangle must touch. (This is basically an application of
             * the Jordan Curve Theorem). The alternative situation is that the test
             * envelope is "on a corner" of the rectangle envelope, i.e. is not
             * completely bisected. In this case it is not possible to make a conclusion
             */
            if (elementEnv.Minimum.X >= _rectEnv.Minimum.X && elementEnv.Maximum.X <= _rectEnv.Maximum.X)
            {
                Intersects = true;
                return;
            }
            if (elementEnv.Minimum.Y >= _rectEnv.Minimum.Y && elementEnv.Maximum.Y <= _rectEnv.Maximum.Y)
            {
                Intersects = true;
            }
        }

        #endregion
    }

    /// <summary>
    /// A visitor which tests whether it can be 
    /// concluded that a geometry contains a vertex of
    /// a query geometry.
    /// </summary>
    /// <author>Martin Davis</author>
    internal class GeometryContainsPointVisitor : ShortCircuitedGeometryVisitor
    {
        #region Fields

        private readonly IEnvelope _rectEnv;
        private readonly ICoordinateSequence _rectSeq;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rectangle"></param>
        public GeometryContainsPointVisitor(IPolygon rectangle)
        {
            _rectSeq = rectangle.Shell.CoordinateSequence;
            _rectEnv = rectangle.EnvelopeInternal;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether it can be concluded that a corner point of the rectangle is
        /// contained in the geometry, or whether further testing is required.
        /// </summary>
        /// <returns><value>true</value> if a corner point is contained 
        /// or <value>false</value> if no conclusion about intersection can be made
        /// </returns>
        public bool ContainsPoint { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool IsDone()
        {
            return ContainsPoint;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        protected override void Visit(IGeometry geom)
        {
            if (!(geom is IPolygon))
                return;
            
            var elementEnv = geom.EnvelopeInternal;
            if (! _rectEnv.Intersects(elementEnv))
                return;
            
            // test each corner of rectangle for inclusion
            var rectPt = new Coordinate();
            for (var i = 0; i < 4; i++)
            {
                _rectSeq.GetCoordinate(i, rectPt);
                if (!elementEnv.Contains(rectPt))
                    continue;
                
                // check rect point in poly (rect is known not to touch polygon at this point)
                if (SimplePointInAreaLocator.ContainsPointInPolygon(rectPt, (IPolygon) geom))
                {
                    ContainsPoint = true;
                    return;
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// A visitor to test for intersection between the query rectangle and the line segments of the geometry.
    /// </summary>
    /// <author>Martin Davis</author>
    internal class RectangleIntersectsSegmentVisitor : ShortCircuitedGeometryVisitor
    {
        #region Fields

        private readonly Coordinate _p0 = new Coordinate();
        private readonly Coordinate _p1 = new Coordinate();
        private readonly IEnvelope _rectEnv;
        private readonly RectangleLineIntersector _rectIntersector;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a visitor for checking rectangle intersection with segments
        /// </summary>
        /// <param name="rectangle">the query rectangle </param>
        public RectangleIntersectsSegmentVisitor(IPolygon rectangle)
        {
            _rectEnv = rectangle.EnvelopeInternal;
            _rectIntersector = new RectangleLineIntersector(_rectEnv);
        }

        #endregion

        #region Properties

        /// <summary>Reports whether any segment intersection exists.</summary>
        /// <returns>true if a segment intersection exists or
        /// false if no segment intersection exists</returns>
        public bool Intersects { get; private set; }

        #endregion

        #region Methods

        private void CheckIntersectionWithLineStrings(IEnumerable<IGeometry> lines)
        {
            foreach (ILineString testLine in lines)
            {
                CheckIntersectionWithSegments(testLine);
                if (Intersects)
                    return;
            }
        }

        private void CheckIntersectionWithSegments(ICurve testLine)
        {
            var seq1 = testLine.CoordinateSequence;
            for (var j = 1; j < seq1.Count; j++)
            {
                seq1.GetCoordinate(j - 1, _p0);
                seq1.GetCoordinate(j, _p1);

                if (!_rectIntersector.Intersects(_p0, _p1)) continue;
                Intersects = true;
                return;
            }
        }

        protected override bool IsDone()
        {
            return Intersects;
        }

        protected override void Visit(IGeometry geom)
        {
            /**
             * It may be the case that the rectangle and the 
             * envelope of the geometry component are disjoint,
             * so it is worth checking this simple condition.
             */
            var elementEnv = geom.EnvelopeInternal;
            if (!_rectEnv.Intersects(elementEnv))
                return;

            // check segment intersections
            // get all lines from geometry component
            // (there may be more than one if it's a multi-ring polygon)
            var lines = LinearComponentExtracter.GetLines(geom);
            CheckIntersectionWithLineStrings(lines);
        }

        #endregion
    }
}