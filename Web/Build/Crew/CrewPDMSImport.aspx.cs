using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using OfficeOpenXml;
using Telerik.Web.UI;
public partial class Crew_CrewPDMSImport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Upload", "UPLOAD",ToolBarDirection.Right);
        MenuPDMSImort.AccessRights = this.ViewState;
        MenuPDMSImort.MenuList = toolbar.Show();

    }

    protected void MenuPDMSImort_OnTabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToString().ToUpper() == "UPLOAD")
        {
            try
            {
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(FileUpload.FileName.ToString());

                if (extension.ToUpper() == ".TXT")
                {
                    string strpath = HttpContext.Current.Server.MapPath("~/Attachments/Crew/") + fileName + extension;
                    FileUpload.PostedFile.SaveAs(strpath);
                    bool importstatus = false;
                    CheckImportfile(strpath, ref importstatus);

                    if (importstatus)
                    {
                        if (rblType.SelectedValue == "1")
                        {
                            EmployeeUpdate(strpath, fileName);
                        }
                        if (rblType.SelectedValue == "2")
                        {
                            FamilyUpdate(strpath, fileName);
                        }
                    }
                    else
                    {
                        ucError.ErrorMessage = "Please upload correct file with data.";
                        ucError.Visible = true;
                        return;
                    }
                }
                else
                {
                    ucError.ErrorMessage = "Please upload .txt file only";
                    ucError.Visible = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }

    private void CheckImportfile(string filepath, ref bool importstatus)
    {
        if (File.Exists(filepath))
            importstatus = true;
        else
            importstatus = false;
    }

    public void FamilyUpdate(string filepath, string FileGuidName)
    {
        try
        {
            string[] lines = System.IO.File.ReadAllLines(filepath);

            List<string> cellvalues = new List<string>();

            int linecount = lines.Length;
            int rowcount = 0;

            foreach (string line in lines)
            {
                string[] data = line.Split('|');
                int count = data.Length;

                for (int i = 0; i < count; i++)
                {
                    cellvalues.Add(data[i]);
                }

                if (rowcount == 0)
                {
                    if (!VerifyFamilyHeader(cellvalues))
                    {
                        ucError.ErrorMessage = "File is of incorrect format";
                        ucError.Visible = true;
                        return;
                    }

                    if (count - 1 > 0)
                    {
                        PhoenixCrewReportsDataExport.DataImportFlag(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 2);
                    }

                    rowcount++;
                    cellvalues.Clear();
                    continue;
                }

                UpdateFamilyDetails(cellvalues);
                cellvalues.Clear();
            }


        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

    public void EmployeeUpdate(string filepath, string FileGuidName)
    {
        try
        {
            string[] lines = System.IO.File.ReadAllLines(filepath);

            List<string> cellvalues = new List<string>();

            int linecount = lines.Length;
            int rowcount = 0;

            foreach (string line in lines)
            {
                string[] data = line.Split('|');
                int count = data.Length;

                for (int i = 0; i < count; i++)
                {
                    cellvalues.Add(data[i]);
                }

                if (rowcount == 0)
                {
                    if (!VerifyHeader(cellvalues))
                    {
                        ucError.ErrorMessage = "File is of incorrect format";
                        ucError.Visible = true;
                        return;
                    }

                    if (count - 1 > 0)
                    {
                        PhoenixCrewReportsDataExport.DataImportFlag(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
                    }

                    rowcount++;
                    cellvalues.Clear();
                    continue;
                }
               
                UpdateEmployeeDetails(cellvalues);
                cellvalues.Clear();
            }
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

    protected bool VerifyHeader(List<string> li)
    {
        if (!li[0].ToString().ToUpper().Equals("GLOBALEMPCODE"))
            return false;
        else if (!li[1].ToString().ToUpper().Equals("FIRSTNAME"))
            return false;
        else if (!li[2].ToString().ToUpper().Equals("MIDDLENAME"))
            return false;
        else if (!li[3].ToString().ToUpper().Equals("LASTNAME"))
            return false;
        else if (!li[4].ToString().ToUpper().Equals("BIRTHDATE"))
            return false;
        else if (!li[5].ToString().ToUpper().Equals("GENDERCODE"))
            return false;
        else if (!li[6].ToString().ToUpper().Equals("NATIONCODE"))
            return false;
        else if (!li[7].ToString().ToUpper().Equals("PASSPORTNO"))
            return false;
        else if (!li[8].ToString().ToUpper().Equals("CDCNO"))
            return false;
        else if (!li[9].ToString().ToUpper().Equals("NATIONCODE2"))
            return false;
        else if (!li[10].ToString().ToUpper().Equals("HIREDATE"))
            return false;
        else if (!li[11].ToString().ToUpper().Equals("RANK"))
            return false;
        else if (!li[12].ToString().ToUpper().Equals("EMAIL"))
            return false;
        else if (!li[13].ToString().ToUpper().Equals("MOBILENUMBER"))
            return false;
        else if (!li[14].ToString().ToUpper().Equals("PHONENUMBER"))
            return false;
        else if (!li[15].ToString().ToUpper().Equals("PERMANENTADDRESS1"))
            return false;
        else if (!li[16].ToString().ToUpper().Equals("PERMANENTADDRESS2"))
            return false;
        else if (!li[17].ToString().ToUpper().Equals("PERMANENTADDRESS3"))
            return false;
        else if (!li[18].ToString().ToUpper().Equals("PERMANENTCITY"))
            return false;
        else if (!li[19].ToString().ToUpper().Equals("PERMANENTSTATE"))
            return false;
        else if (!li[20].ToString().ToUpper().Equals("PERMANENTCOUNTRY"))
            return false;
        else if (!li[21].ToString().ToUpper().Equals("PERMANENTPOSTALCODE"))
            return false;
        else if (!li[22].ToString().ToUpper().Equals("PERMANENTPHONE2"))
            return false;
        else if (!li[23].ToString().ToUpper().Equals("LOCALADDRESS1"))
            return false;
        else if (!li[24].ToString().ToUpper().Equals("LOCALADDRESS2"))
            return false;
        else if (!li[25].ToString().ToUpper().Equals("LOCALADDRESS3"))
            return false;
        else if (!li[26].ToString().ToUpper().Equals("LOCALCITY"))
            return false;
        else if (!li[27].ToString().ToUpper().Equals("LOCALSTATE"))
            return false;
        else if (!li[28].ToString().ToUpper().Equals("LOCALCOUNTRY"))
            return false;
        else if (!li[29].ToString().ToUpper().Equals("LOCALPOSTALCODE"))
            return false;
        else if (!li[30].ToString().ToUpper().Equals("PERMANENTPHONE2"))
            return false;
        else if (!li[31].ToString().ToUpper().Equals("SOURCE"))
            return false;
        else
            return true;
    }

    protected bool VerifyFamilyHeader(List<string> li)
    {
        if (!li[0].ToString().ToUpper().Equals("GLOBALEMPCODE"))
            return false;
        else if (!li[1].ToString().ToUpper().Equals("FIRSTNAME"))
            return false;
        else if (!li[2].ToString().ToUpper().Equals("LASTNAME"))
            return false;
        else if (!li[3].ToString().ToUpper().Equals("RELATIONSHIP"))
            return false;
        else if (!li[4].ToString().ToUpper().Equals("GENDERCODE"))
            return false;
        else if (!li[5].ToString().ToUpper().Equals("NATIONALITY"))
            return false;
        else if (!li[6].ToString().ToUpper().Equals("ADDRESS1"))
            return false;
        else if (!li[7].ToString().ToUpper().Equals("ADDRESS2"))
            return false;
        else if (!li[8].ToString().ToUpper().Equals("ADDRESS3"))
            return false;
        else if (!li[9].ToString().ToUpper().Equals("CITY"))
            return false;
        else if (!li[10].ToString().ToUpper().Equals("STATE"))
            return false;
        else if (!li[11].ToString().ToUpper().Equals("COUNTRY"))
            return false;
        else if (!li[12].ToString().ToUpper().Equals("POSTALCODE"))
            return false;
        else if (!li[13].ToString().ToUpper().Equals("STDCODE"))
            return false;
        else if (!li[14].ToString().ToUpper().Equals("PHONENUMBER"))
            return false;
        else if (!li[15].ToString().ToUpper().Equals("NEXTOFKIN"))
            return false;
        else if (!li[16].ToString().ToUpper().Equals("SOURCE"))
            return false;
        else
            return true;
    }

    protected void UpdateFamilyDetails(List<string> li)
    {
        PhoenixCrewReportsDataExport.DataImportFamily(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , li[0].ToString()
                                                        , li[1].ToString()
                                                        , li[2].ToString()
                                                        , li[3].ToString()
                                                        , li[4].ToString()
                                                        , li[5].ToString()
                                                        , li[6].ToString()
                                                        , li[7].ToString()
                                                        , li[8].ToString()
                                                        , li[9].ToString()
                                                        , li[10].ToString()
                                                        , li[11].ToString()
                                                        , li[12].ToString()
                                                        , li[13].ToString()
                                                        , li[14].ToString()
                                                        , li[15].ToString()
                                                        , li[16].ToString()
                                                        );
    }

    private void UpdateEmployeeDetails(List<string> li)
    {
        try
        {
            PhoenixCrewReportsDataExport.DataImportPersonal(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            li[0].ToString(),//globalempcode
                                                            li[1].ToString(),//first name
                                                            li[2].ToString(),//middle name
                                                            li[3].ToString(),//last name
                                                            General.GetNullableDateTime(li[4].ToString()),
                                                            li[5].ToString(),//gendercode
                                                            li[6].ToString(),//nationcode
                                                            li[7].ToString(),//passportno
                                                            li[8].ToString(),//seamanbook
                                                            li[9].ToString(),//nationcode2
                                                            General.GetNullableDateTime(li[10].ToString()),//hiredate
                                                            li[11].ToString(),//rankname
                                                            li[12].ToString(),//email
                                                            li[13].ToString(),//mobilenumber
                                                            li[14].ToString(),//phonenumber
                                                            li[15].ToString(),//permanentaddress1
                                                            li[16].ToString(),//permanentaddress2
                                                            li[17].ToString(),//permanentaddress3
                                                            li[18].ToString(),//permanentcity
                                                            li[19].ToString(),//permanentstate
                                                            li[20].ToString(),//permanentcountry
                                                            li[21].ToString(),//permanentpostalcode
                                                            li[22].ToString(),//localaddress1
                                                            li[23].ToString(),//localaddress2
                                                            li[24].ToString(),//localaddress3
                                                            li[25].ToString(),//localcity
                                                            li[26].ToString(),//localstate
                                                            li[27].ToString(),//localcountry
                                                            li[28].ToString(),//localpostalcode
                                                            li[29].ToString(),//permanentphone2
                                                            li[30].ToString()//source
                                                            );

            ucStatus.Text = "Uploaded Successfully";
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

    protected void txtFileUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
    {
        try
        {
            string targetFolder = ((RadAsyncUpload)sender).TargetFolder;
            string[] lines = System.IO.File.ReadAllLines(targetFolder);

            List<string> cellvalues = new List<string>();

            int linecount = lines.Length;
            int rowcount = 0;

            foreach (string line in lines)
            {
                string[] data = line.Split('|');
                int count = data.Length;

                for (int i = 0; i < count; i++)
                {
                    cellvalues.Add(data[i]);
                }

                if (rowcount == 0)
                {
                    if (!VerifyHeader(cellvalues))
                    {
                        ucError.ErrorMessage = "File is of incorrect format";
                        ucError.Visible = true;
                        return;
                    }

                    if (count - 1 > 0)
                    {
                        PhoenixCrewReportsDataExport.DataImportFlag(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
                    }

                    rowcount++;
                    cellvalues.Clear();
                    continue;
                }

                UpdateEmployeeDetails(cellvalues);
                cellvalues.Clear();
            }
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }
}
