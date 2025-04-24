using Grasshopper.Kernel;
using System;

namespace YYMSimple
{
    public class Simple_Mathematics : GH_Component
    {
        public Simple_Mathematics() : base("Mathematics", "Math", "Simple_Mathematics", "Extra", "Simple")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Angle", "A", "The angle to measure", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Radians", "R", "Work in Radians", GH_ParamAccess.item, true); // The default value is 'true'
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Sin", "sin", "The sine of the Angle.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Cos", "cos", "The cosine of the Angle.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Tan", "tan", "The tangent of the Angle.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Declare variables to contain all inputs.
            // We can assign some initial values that are either sensible or indicative.
            double angle = double.NaN;
            bool radians = false;

            // Use the DA object to retrieve the data inside the input parameters.
            // If the retrieval fails (for example if there is no data) we need to abort.
            if (!DA.GetData(0, ref angle)) { return; }
            if (!DA.GetData(1, ref radians)) { return; }

            // If the angle value is not a valid number, we should abort.
            if (!Rhino.RhinoMath.IsValidDouble(angle)) { return; }

            // If the user wants to work in degrees rather than radians, 
            // we assume that angle is defined in degrees. 
            // We need to convert it into Radians again.
            if (!radians)
            {
                angle = Rhino.RhinoMath.ToRadians(angle);
            }

            // Now we are ready to assign the outputs via the DA object.
            // Since the Sin(), Cos() and Tan() never fail, we might as well 
            // combine them with the assignment.
            DA.SetData(0, Math.Sin(angle));
            DA.SetData(1, Math.Cos(angle));
            DA.SetData(2, Math.Tan(angle));
        }

        public override Guid ComponentGuid => new Guid("b08849ed-c428-41e6-ba47-769fe5e01b3b");
    }
}