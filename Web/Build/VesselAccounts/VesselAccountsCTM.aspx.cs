using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class VesselAccountsCTM : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCTM.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvCTM')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarmain.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCTM.aspx", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "NEW");
            MenuCTM.AccessRights = this.ViewState;
            MenuCTM.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {

                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                PhoenixVesselAccountsCTM.CaptaiCashManipulateRecord(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                gvCTM.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCTM.SelectedIndexes.Clear();
        gvCTM.EditIndexes.Clear();
        gvCTM.DataSource = null;
        gvCTM.Rebind();
    }
    protected void MenuCTM_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                string[] alColumns = { "FLDCTMPURPOSEDESC", "FLDDATE", "FLDSEAPORTNAME", "FLDETA", "FLDSUPPLIERNAME", "FLDAMOUNT", "FLDRECEIVEDDATE", "FLDRECEIVEDAMOUNT", "FLDSTATUS" };
                string[] alCaptions = { "CTM Purpose", "Date", "Required Port", "ETA", "Port Agent", "CTM Amount", "Received Date", "Received Amount", "Status" };

                DataSet ds = PhoenixVesselAccountsCTM.SearchCTMRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                           , null, null
                           , sortexpression, sortdirection
                            , 1
                            , iRowCount, ref iRowCount, ref iTotalPageCount);
                General.ShowExcel("CTM Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                    Response.Redirect("../VesselAccounts/VesselAccountsCTMOffshoreGeneral.aspx", false);
                else
                    Response.Redirect("../VesselAccounts/VesselAccountsCTMGeneral.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDCTMPURPOSEDESC", "FLDDATE", "FLDSEAPORTNAME", "FLDETA", "FLDSUPPLIERNAME", "FLDAMOUNT", "FLDRECEIVEDDATE", "FLDRECEIVEDAMOUNT", "FLDSTATUS" };
            string[] alCaptions = { "CTM Purpose", "Date", "Required Port", "ETA", "Port Agent", "CTM Amount", "Received Date", "Received Amount", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixVesselAccountsCTM.SearchCTMRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , null, null
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , gvCTM.PageSize, ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvCTM", "CTM Request", alCaptions, alColumns, ds);
            gvCTM.DataSource = ds;
            gvCTM.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCTM_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCTM.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCTM_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {


                string ctmid = ((RadLabel)e.Item.FindControl("lblCaptaincashid")).Text;
                string activey = ((RadLabel)e.Item.FindControl("lblEditable")).Text;
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                    Response.Redirect("../VesselAccounts/VesselAccountsCTMOffshoreGeneral.aspx?CTMID=" + ctmid + "&a=" + activey, false);
                else
                    Response.Redirect("../VesselAccounts/VesselAccountsCTMGeneral.aspx?CTMID=" + ctmid + "&a=" + activey, false);
            }

            if (e.CommandName == "DELETE")
            {
                Guid? id = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblCaptaincashid")).Text);
                PhoenixVesselAccountsCTM.DeleteCaptainCash(id.Value);
                Rebind();
            }

            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
}