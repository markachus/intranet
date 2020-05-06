using Intranet.App.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.App.LocalStorage
{
    public interface ILocalEtiquetaRepository
    {
        IEnumerable<EtiquetaModel> GetTags();
        EtiquetaModel GetTag(string name);
        void AddNewTag(EtiquetaModel newTag);
    }
}
