using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class RegisterProsperCardstatusMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperCardstatusMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCardstatus')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperCardstatusMapping.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperCardstatusMapping.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuRegisterCardstatusMapping.AccessRights = this.ViewState;
            MenuRegisterCardstatusMapping.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CardstatusMappingID"] = null;
                BindCardstatus();
                ucRank.SelectedRank = "Dummy";
                gvCardstatus.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }
    protected void BindCardstatus()
    {
        ddlcardstatus.Items.Clear();
        ddlcardstatus.DataSource = PhoenixRegisterProsperCardstatusMapping.ProsperCardstatusList();
        ddlcardstatus.DataTextField = "FLDCARDSTATUSNAME";
        ddlcardstatus.DataValueField = "FLDCARDSTATUSID";
        ddlcardstatus.DataBind();
        ddlcardstatus.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        ddlcardstatus.SelectedIndex = 0;
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
    protected void gvCardstatus_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCardstatus.CurrentPageIndex + 1;
            BindData();
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
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCARDSTATUSNAME", "FLDRANKNAME", "FLDMINPOINTSREQUIRED", "FLDMAXPOINTSREQUIRED" };
            string[] alCaptions = { "Cardstatus Name", "Rank","Min Points Required", "Max Points Required" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            DataSet ds = new DataSet();
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            int? rankid;

            if (General.GetNullableInteger(ucRank.SelectedRank) != null)
                rankid = General.GetNullableInteger(ucRank.SelectedRank).Value;
            else
                rankid = null;

            ds = PhoenixRegisterProsperCardstatusMapping.ProsperCardstatusMapping(
                General.GetNullableGuid(ddlcardstatus.SelectedValue.ToString())
                ,General.GetNullableInteger(rankid.ToString())
                , sortexpression
                ,sortdirection
                ,(int)ViewState["PAGENUMBER"]
                ,gvCardstatus.PageSize
                ,ref iRowCount
                ,ref iTotalPageCount
                );

            General.SetPrintOptions("gvCardstatus", "Cardstatus Mapping", alCaptions, alColumns, ds);

            gvCardstatus.DataSource = ds;
            gvCardstatus.VirtualItemCount = iRowCount;
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCardstatus_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            RadLabel lblCardstatusMappingid = (RadLabel)e.Item.FindControl("lblcardstatusmappingid");
            LinkButton db = (LinkButton)e.Item.FindControl("lnkcardstatusname");
            if (db != null)
            {
                db.Attributes.Add("onclick", "openNewWindow('Cardstatus', '','" + Session["sitepath"] + "/Registers/RegisterProsperCardstatusMappingEdit.aspx?cardstatusmappingid=" + lblCardstatusMappingid.Text + "'); return false;");
            }
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "openNewWindow('Cardstatus', '','" + Session["sitepath"] + "/Registers/RegisterProsperCardstatusMappingEdit.aspx?cardstatusmappingid=" + lblCardstatusMappingid.Text + "'); return false;");
            }
        }
    }
    protected void gvCardstatus_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["CardstatusMappingID"] = ((RadLabel)e.Item.FindControl("lblcardstatusmappingid")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RegisterCardstatusMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if(CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlcardstatus.SelectedIndex = 0;
                ucRank.SelectedRank = "Dummy";
                Rebind();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                if(!IsValid(ddlcardstatus.SelectedValue.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                String scriptpopup = String.Format("javascript:parent.openNewWindow('CardstatusMapping','','" + Session["sitepath"] + "/Registers/RegisterProsperCardstatusMappingAdd.aspx?cardstatusid=" + ddlcardstatus.SelectedValue.ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();
            string[] alColumns = { "FLDCARDSTATUSNAME", "FLDRANKNAME", "FLDMINPOINTSREQUIRED", "FLDMAXPOINTSREQUIRED" };
            string[] alCaptions = { "Cardstatus Name", "Rank", "Min Points Required", "Max Points Required" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            int? rankid;

            if (General.GetNullableInteger(ucRank.SelectedRank) != null)
                rankid = General.GetNullableInteger(ucRank.SelectedRank).Value;
            else
                rankid = null;

            ds = PhoenixRegisterProsperCardstatusMapping.ProsperCardstatusMapping(
                General.GetNullableGuid(ddlcardstatus.SelectedValue.ToString())
                , General.GetNullableInteger(rankid.ToString())
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvCardstatus.PageSize
                , ref iRowCount
                , ref iTotalPageCount
                );

            General.ShowExcel("Cardstatus Mapping", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixRegisterProsperCardstatusMapping.DeleteCardStatusMapping( Guid.Parse(ViewState["CardstatusMappingID"].ToString()));
            Rebind();
            ucStatus.Text = "Information Deleted.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValid(string CardStatus)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(CardStatus) == null)
            ucError.ErrorMessage = "CardStatus Name is required.";

        return (!ucError.IsError);
    }
    protected void gvCardstatus_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void Rebind()
    {
        gvCardstatus.SelectedIndexes.Clear();
        gvCardstatus.EditIndexes.Clear();
        gvCardstatus.DataSource = null;
        gvCardstatus.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
}