using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SoonLearning.Math.Data;

namespace Math.Basic.Data.Geometry
{
    internal class GeometryTypeCollection : ObservableCollection<MathSubTypeItem>
    {
        internal GeometryTypeCollection()
        {
            this.Add(new MathSubTypeItem("线段、直线和射线", MathSubType.Line));
            this.Add(new MathSubTypeItem("角", MathSubType.Angle));
            this.Add(new MathSubTypeItem("角的分类", MathSubType.SortOfAngle));
            this.Add(new MathSubTypeItem("垂线", MathSubType.VerticalLine));
            this.Add(new MathSubTypeItem("平行线", MathSubType.ParallelLine));
            this.Add(new MathSubTypeItem("三角形", MathSubType.Triangle));
            this.Add(new MathSubTypeItem("三角形的分类", MathSubType.SortOfTriangle));
            this.Add(new MathSubTypeItem("四边形", MathSubType.Quadrangle));
            this.Add(new MathSubTypeItem("圆", MathSubType.Circle));
            this.Add(new MathSubTypeItem("轴对称图形", MathSubType.SymmetricFigure));
            this.Add(new MathSubTypeItem("周长", MathSubType.Girth));
            this.Add(new MathSubTypeItem("面积", MathSubType.Area));
            this.Add(new MathSubTypeItem("表面积", MathSubType.SurfaceArea));
            this.Add(new MathSubTypeItem("体积", MathSubType.Volume));
            this.Add(new MathSubTypeItem("长方体、正方体", MathSubType.CuboidAndCube));
            this.Add(new MathSubTypeItem("圆柱", MathSubType.Column));
            this.Add(new MathSubTypeItem("圆周率π", MathSubType.CircumferenceRatio));
            this.Add(new MathSubTypeItem("圆锥的高", MathSubType.HeightOfTaper));
        }
    }
}
