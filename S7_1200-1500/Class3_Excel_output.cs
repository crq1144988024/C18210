using C18210.Class_Tools;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace C18210
{
   public class Class3_Excel_output
    {
        public void PrintReporter(string path, string name, DataGridView datagridview1, int check_num)

        {//skinTextBox1.Text
         //MessageBox.Show(skinComboBox11.Text);return;
            string file_path = path+"\\"+name;
            if (path == null)
            {

               // FolderDialog_file fdialog = new FolderDialog_file();
                //tbFilePath = dialog.FileName;EXCEL表格文件(*.txt)|*.txt|所有文件(*.*)|*.*”c
                //fdialog. file_path_save("EXCEL表格文件(*.xls)|*.xls", out file_path);
              //  fdialog.file_path_save("EXCEL表格文件(*.xlsx)|*.xlsx", name, out path);
              //  path = file_path;
            }



            string destinationFile = file_path;
            string sourceFile = System.AppDomain.CurrentDomain.BaseDirectory + "Excel\\laser模板.xlsx";
            FileInfo file = new FileInfo(sourceFile);
            if (file.Exists)
            {
                file.CopyTo(destinationFile, true);
            }


            var newFile = new FileInfo(destinationFile);


            using (var package = new ExcelPackage(newFile))

            {


                {
                    int worksheet_now = 1;

                    //   CreateWorksheetAbAssets(package.Workbook.Worksheets.Copy("Sheet0", kSheetNameAbAssets));

                    //     FillWorksheetAbAssets(package.Workbook.Worksheets[worksheet_now + 1], datagridview1, check_num);
                    FillWorksheetAbAssets(package.Workbook.Worksheets["Sheet0"], datagridview1, check_num);
                    //package.Workbook.Worksheets.Delete("Sheet0");
                    package.Save();

                  //  MessageBox.Show("导出成功！");
                }
            }

        }
        public static void CreateWorksheetAbAssets(ExcelWorksheet ws)

        {
            return;
            ws.TabColor = ColorTranslator.FromHtml("#32b1fa");

            // 标签颜色
            // 全体颜色
            ws.Cells.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#3d4d65"));

            {

                // 边框样式

                var border = ws.Cells.Style.Border;

                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                // 边框颜色
                var clr = ColorTranslator.FromHtml("#cad7e2");

                border.Bottom.Color.SetColor(clr);

                border.Top.Color.SetColor(clr);

                border.Left.Color.SetColor(clr);


                border.Right.Color.SetColor(clr);
            }
        }
        private const string kSheetNameAbAssets = "Sheet1";

        private const string kSheetNameAbDetail = "Sheet2";
        public void FillWorksheetAbAssets(ExcelWorksheet ws, DataGridView DataGridView_BOM_Hold, int check_num)
        {

            if (DataGridView_BOM_Hold.Rows.Count <= 0) { return; }


            int k = 2;
            int r = 0;
            for (int i = 0; i < DataGridView_BOM_Hold.Rows.Count; i++)
            {
                //DataGridViewCheckBoxCell chkBoxCell = (DataGridViewCheckBoxCell)DataGridView_BOM_Hold.Rows[i].Cells[check_num];
                //bool temp = false;

                //if (chkBoxCell != null && ((bool)chkBoxCell.EditingCellFormattedValue == true || (bool)chkBoxCell.FormattedValue == true))
                //{
                //    temp = true;
                //}

                //if (temp == true)
                //{
                Color color_temp = new Color();
                string str_result = DataGridView_BOM_Hold.Rows[i].Cells[3].Value.ToString();
                color_temp = Color.White;
                if (str_result.Contains("OK"))
                {
                    color_temp = System.Drawing.Color.Green;
                }
                else if (str_result.Contains("NG"))
                {
                    color_temp = System.Drawing.Color.Red;
                }


             

                    for (int p = 0; p < 16; p++)
                    {
                        ws.Cells[k, p+1].Value = DataGridView_BOM_Hold.Rows[i].Cells[p].Value;
                        ws.Cells[k, p + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[k, p + 1].Style.Fill.BackgroundColor.SetColor(color_temp);
                  
                    }
                
                    r++;
                    k++;
                //}


            }


            //   ws.Cells[3, 1].Hyperlink = new ExcelHyperLink(kSheetNameAbDetail + "!A3", "SubTerrainObjs_1_1.assetbundle");


            // ws.Cells[4, 1].Hyperlink = new ExcelHyperLink(kSheetNameAbDetail + "!A300", "Terrain_Data_1_8.assetbundle");



        }
    }
}
