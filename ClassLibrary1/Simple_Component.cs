using Grasshopper.Kernel;
using System;

namespace YYMSimple
{
    public class Simple_component : GH_Component
    {
        public Simple_component() : base("MyFirst", "MFC", "My first component", "Extra", "Simple")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("String", "S", "String to reverse", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Reverse", "R", "Reversed string", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Declare a variable for the input String
            string data = null;

            // Use the DA object to retrieve the data inside the first input parameter.
            // If the retieval fails (for example if there is no data) we need to abort.
            if (!DA.GetData(0, ref data)) { return; }

            // If the retrieved data is Nothing, we need to abort.
            // We're also going to abort on a zero-length String.
            if (data == null) { return; }
            if (data.Length == 0) { return; }

            // Convert the String to a character array.
            char[] chars = data.ToCharArray();

            // Reverse the array of character.
            System.Array.Reverse(chars);

            // Use the DA object to assign a new String to the first output parameter.
            DA.SetData(0, new string(chars));
        }

        public override Guid ComponentGuid => new Guid("12345678-ABCD-1234-ABCD-1234567890AB");
    }
}