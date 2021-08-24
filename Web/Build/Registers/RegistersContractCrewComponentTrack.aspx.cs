using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using Telerik.Web.UI;

public partial class RegistersContractCrewComponentTrack : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersContractCrewComponentTrack.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewComponentTrack')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersContractCrewComponentTrack.aspx", "Find", "<i class=\"fas fa-search\"></i>", "Find");
            MenuRegistersCity.AccessRights = this.ViewState;
            MenuRegistersCity.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrewComponentTrack.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        gvCrewComponentTrack.SelectedIndexes.Clear();
        gvCrewComponentTrack.EditIndexes.Clear();
        gvCrewComponentTrack.DataSource = null;
        gvCrewComponentTrack.Rebind();
    }

    protected void RegistersCity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDNAME", "FLDISACTIVEYN" };
                string[] alCaptions = { "Name", "Active YN" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                DataSet ds = PhoenixRegistersContractComponentTracking.CrewComponentTrackSearch(txtName.Text.Trim()
                                                                                                , sortexpression, sortdirection
                                                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                               General.ShowRecords(null),
                                                                                                ref iRowCount,
                                                                                                ref iTotalPageCount);
                General.ShowExcel("Component Track", ds.Tables[0], alColumns, alCaptions, null, string.Empty);
            }
            if (CommandName.ToUpper().Equals("Find"))
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                Rebind();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME", "FLDISACTIVEYN" };
        string[] alCaptions = { "Name", "Active YN" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersContractComponentTracking.CrewComponentTrackSearch(txtName.Text.Trim()
                                                                                        , sortexpression, sortdirection
                                                                                        , Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                       gvCrewComponentTrack.PageSize,
                                                                                        ref iRowCount,
                                                                                        ref iTotalPageCount);

        gvCrewComponentTrack.DataSource = ds.Tables[0];
        gvCrewComponentTrack.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvCrewComponentTrack", "Component Track", alCaptions, alColumns, ds);
    }
    protected void gvCrewComponentTrack_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                string active = ((RadCheckBox)e.Item.FindControl("chkAddactiveyn")).Checked == true ? "1" : "0";
                string name = ((RadTextBox)e.Item.FindControl("txtAddName")).Text.Trim();
                if (!IsValidComponent(name))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContractComponentTracking.InsertCrewComponentTrack(name, int.Parse(active));
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string dtkey = ((RadLabel)e.Item.FindControl("lbleditdtkey")).Text.Trim();
                string active = ((RadCheckBox)e.Item.FindControl("chkEditactiveyn")).Checked == true ? "1" : "0";
                string name = ((RadTextBox)e.Item.FindControl("txtEditName")).Text.Trim();
                if (!IsValidComponent(name))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContractComponentTracking.UpdateCrewComponentTrack(new Guid(dtkey), name, int.Parse(active));
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
    protected void gvCrewComponentTrack_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewComponentTrack.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidComponent(string name)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = " Name is required.";
        return (!ucError.IsError);
    }
    protected void gvCrewComponentTrack_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            LinkButton ob = (LinkButton)e.Item.FindControl("cmdmapping");
            RadLabel trackid = (RadLabel)e.Item.FindControl("lblComponentId");
            if (ob != null)
                ob.Attributes.Add("onclick", "javascript:openNewWindow('cmdmapping','','" + Session["sitepath"] + "/Registers/RegistersContractCrewComponentTrackMapping.aspx?trackid=" + trackid.Text + "')");
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
}
