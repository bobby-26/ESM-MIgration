using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.IO;
using SouthNests.Phoenix.CrewCommon;
using System.Xml;
using System.Text;
using Telerik.Web.UI;
public partial class DataTranserEPSSImport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
         
            toolbar.AddButton("EPSS", "EPSS", ToolBarDirection.Right);
            toolbar.AddButton("Vessel List", "VESSELLIST", ToolBarDirection.Right);
            MenuEPSSList.AccessRights = this.ViewState;
            MenuEPSSList.MenuList = toolbar.Show();
            MenuEPSSList.SelectedMenuIndex = 0;

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../DataTransfer/DataTranserEPSSImport.aspx", "Import EPSS", "import-history.png", "IMPORT");            
            MenuEPSSImport.AccessRights = this.ViewState;
            MenuEPSSImport.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = 1;
                gvImp.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuEPSSList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("VESSELLIST"))
        {
            Response.Redirect("DataTransferVesselList.aspx", true);
        }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void MenuEPSSImport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("IMPORT"))
            {
                DataSet ds = PhoenixGeneralSettings.ConfigurationSettingEdit(1);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string syncpath = ds.Tables[0].Rows[0]["FLDMAILEXEPATH"].ToString();                  
                    syncpath = syncpath.TrimEnd(new char[] { '/', '\\' });
                    syncpath = syncpath + "\\EPSS\\";
                    if (Directory.Exists(syncpath))
                    {
                        string[] files = Directory.GetFiles(syncpath, "*.xml");
                        if (files.Length == 0)
                        {
                            ucError.ErrorMessage = "EPSS Xml file not found for importing";
                            ucError.Visible = true;
                            return;
                        }
                        foreach (string str in files)
                        {
                            string filename = Path.GetFileName(str);
                            XmlDocument epss = new XmlDocument();
                            epss.Load(str);

                            XmlElement ele = epss.DocumentElement;
                            XmlNodeList list = ele.SelectNodes("/candidates/candidate/course");
                            if (list.Count > 0)
                            {
                                PhoenixEPSSImport.InsertEPSSImport(filename, epss.OuterXml);
                            }
                            if (!Directory.Exists(syncpath + "Archive"))
                                Directory.CreateDirectory("Archive");
                            File.Move(str, syncpath + "Archive\\" + filename.Replace(".xml", "_") + DateTime.Now.ToString("dd-MMM-yyyy hh-mm-ss") + ".xml");
                        }
                        BindData();
                    }
                    else
                    {
                        ucError.ErrorMessage = "EPSS folder didn't exists. Create EPSS folder under Phoenix Sync folder.";
                        ucError.Visible = true;
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            string[] alColumns = { "FLDVESSELNAME", "FLDMONTH", "FLDYEAR", "FLDCREATEDDATE", "FLDTYPENAME" };
            string[] alCaptions = { "Vessel Name", "Month", "Year", "Created Date", "Tyype" };


            DataTable dt = PhoenixEPSSImport.SearchEPSSImport(
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null),
               ref iRowCount,
               ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvImp", "EPSS Import", alCaptions, alColumns, ds);
         
            gvImp.DataSource = dt;
            gvImp.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvImp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvImp.CurrentPageIndex + 1;
        BindData();
    }
}
