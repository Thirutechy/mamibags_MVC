using System;

namespace mamibags.Controllers
{
    public class HttPostedFileBase
    {
        public string FileName { get; internal set; }

        internal void SaveAs(string path)
        {
            throw new NotImplementedException();
        }
    }
}