using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maui.Calc;

namespace Saver
{
    public class GoogleDriveSaver
    {
        private readonly GoogleDriveService _googleDriveService;

        public GoogleDriveSaver()
        {
            _googleDriveService = new GoogleDriveService();
        }

        public async Task SaveToGoogleDriveAsync(IDictionary<string, Cell> sheet, int rows, int columns)
        {
            var saver = new XmlSaver();
            string fileContent = saver.GenerateContent(sheet, rows, columns);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
            string fileName = $"sheet{timestamp}.xml";
            string mimeType ="application/xml";
            await _googleDriveService.UploadAsync(fileName, fileContent, mimeType);
        }
    }
}
