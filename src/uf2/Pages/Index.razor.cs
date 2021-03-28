using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using W8lessLabs.Blazor.LocalFiles;

namespace uf2.Pages
{
    public partial class Index : ComponentBase
    {
        private List<BinaryBlock> Blocks;

        private FileSelect BinFileSelect;
        private string Address;
        private string Uf2FileName;

        private async Task LoadBinFile(SelectedFile[] files)
        {
            var fileName = files[0].Name;

            uint address;
            if (!Helper.ConvertToUint32(Address, out address)) throw new ApplicationException();
            var data = await BinFileSelect.GetFileBytesAsync(fileName);

            if (Blocks == null) Blocks = new List<BinaryBlock>();
            Blocks.Add(new BinaryBlock { Address = address, Data = data });

            if (string.IsNullOrEmpty(Uf2FileName))
            {
                Uf2FileName = Path.GetFileNameWithoutExtension(fileName) + ".uf2";
            }
        }

        private async Task SaveUf2File()
        {
            var uf2 = Uf2Converter.BytesToUf2(Blocks);

            await SaveAsFile(Uf2FileName, uf2);
        }

        [Inject]
        private IJSRuntime JS { get; set; }

        private async Task SaveAsFile(string fileName, byte[] data)
        {
            await JS.InvokeAsync<object>("saveAsFile", fileName, Convert.ToBase64String(data));
        }

    }
}
