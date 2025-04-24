using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Collections.Generic;
using System;

namespace YYMSimple
{
    public class SlabWithHoles : GH_Component
    {
        public SlabWithHoles() : base(
            "楼板开洞",
            "SlabHole",
            "在楼板上生成洞口",
            "建筑工具",
            "结构")
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("楼板轮廓", "S", "楼板外轮廓线", GH_ParamAccess.item);
            pManager.AddCurveParameter("洞口", "H", "洞口轮廓线", GH_ParamAccess.list);
            pManager.AddNumberParameter("厚度", "T", "楼板厚度", GH_ParamAccess.item, 0.2);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("楼板", "B", "带洞口的楼板", GH_ParamAccess.item);
            pManager.AddCurveParameter("洞口线", "C", "洞口边界线", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // 获取输入
            Curve slabBoundary = null;
            List<Curve> holes = new List<Curve>();
            double thickness = 0.2;

            if (!DA.GetData(0, ref slabBoundary)) return;
            DA.GetDataList(1, holes);
            DA.GetData(2, ref thickness);

            // 校验输入
            if (!slabBoundary.IsClosed)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "楼板轮廓必须闭合");
                return;
            }

            // 生成楼板（ extrusion + 布尔运算）
            var extrusion = Extrusion.Create(slabBoundary, thickness, true);
            var brep = extrusion.ToBrep();

            foreach (var hole in holes)
            {
                if (hole.IsClosed)
                {
                    var holeExtrusion = Extrusion.Create(hole, thickness, true);
                    brep = Brep.CreateBooleanDifference(brep, holeExtrusion.ToBrep(), 0.01)[0];
                }
            }

            // 输出
            DA.SetData(0, brep);
            DA.SetDataList(1, holes);
        }

        public override Guid ComponentGuid => new Guid("9c60521e-cd5f-4a5f-976d-7c93b4c61044");
    }
}