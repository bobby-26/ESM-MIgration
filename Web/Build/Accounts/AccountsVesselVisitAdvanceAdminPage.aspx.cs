using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsVesselvisitAdvanceAdminPage : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Visit Advance Admin", "ADVANCEADMIN", ToolBarDirection.Right);
        toolbarmain.AddButton("Visit Claim Admin", "CLAIMADMIN", ToolBarDirection.Right);
        toolbarmain.AddButton("Vessel Visit Admin", "VISITADMIN", ToolBarDirection.Right);

        MenuLineItem.AccessRights = this.ViewState;
        MenuLineItem.MenuList = toolbarmain.Show();

        MenuLineItem.SelectedMenuIndex = 0;

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsVesselvisitAdvanceAdminPage.aspx", "Excel", "icon_xls.png", "EXCEL");
        toolbargrid.AddImageButton("../Accounts/AccountsVesselvisitAdvanceAdminPage.aspx", "Find", "search.png", "FIND");
        MenuAdminPageSub.AccessRights = this.ViewState;
        MenuAdminPageSub.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            DateTime now = DateTime.Now;

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["SelectedVesselList"] = "";
            BindVesselList();
            gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("VISITADMIN"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitAdminPage.aspx");
            }
            if (CommandName.ToUpper().Equals("CLAIMADMIN"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitClaimAdminPage.aspx");
            }
            if (CommandName.ToUpper().Equals("ADVANCEADMIN"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitAdvanceAdminPage.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuAdminPageSub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindVesselList()
    {
        DataSet ds = PhoenixRegistersVessel.ListAllVessel();
        chkVesselList.Items.Add("select");
        chkVesselList.DataSource = ds;
        chkVesselList.DataTextField = "FLDVESSELNAME";
        chkVesselList.DataValueField = "FLDVESSELID";
        chkVesselList.DataBind();
        //chkVesselList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = new DataSet();

            ds = PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceReturnList(
                     General.GetNullableString(txtEmployeeNameSearch.Text),
                     General.GetNullableString(txtAdvancenumber.Text),
                     General.GetNullableString(txtFormNo.Text),
                     General.GetNullableString(ViewState["SelectedVesselList"].ToString()),
                     null,
                     null,
                     null,
                     (int)ViewState["PAGENUMBER"],
                     gvAttachment.PageSize,
                     ref iRowCount,
                     ref iTotalPageCount,
                     null);

            gvAttachment.DataSource = ds;
            gvAttachment.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    protected void chkVesselList_Changed(object sender, EventArgs e)
    {
        ViewState["SelectedVesselList"] = "";
        foreach (ListItem item in chkVesselList.Items)
        {
            if (item.Selected == true && !ViewState["SelectedVesselList"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString() + ',' + item.Value.ToString();
            }
            if (item.Selected == false && ViewState["SelectedVesselList"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString().Replace("," + item.Value.ToString() + ",", ",");
            }
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDFORMNUMBER", "FLDTRAVELADVANCENUMBER", "FLDEMPLOYEENAME", "FLDVESSELNAME", "FLDQUICKNAME" };
        string[] alCaptions = { "Form number", "Advance number", "Employee name", "Vessel name", "Status" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        ds = PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceReturnList(
                     General.GetNullableString(txtEmployeeNameSearch.Text),
                     General.GetNullableString(txtFormNo.Text),
                     null,
                     General.GetNullableString(ViewState["SelectedVesselList"].ToString()),
                     null,
                     null,
                     null,
                     (int)ViewState["PAGENUMBER"],
                    iRowCount,
                     ref iRowCount,
                     ref iTotalPageCount,
                     null);


        Response.AddHeader("Content-Disposition", "attachment; filename= VesselVisit.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Visit </h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void gvAttachment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SAVE"))
        {

            if (((UserControlQuick)e.Item.FindControl("ucAdvancestatus")).SelectedValue.ToString().ToUpper().Equals("DUMMY"))
            {
                ucError.ErrorMessage = "Status is required.";
                ucError.Visible = true;
                return;
            }

            PhoenixAccountsVesselVisitITSuperintendentRegister.VisitAdvanceStatusUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(((RadLabel)e.Item.FindControl("lblvisitidEdit")).Text),
                General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucAdvancestatus")).SelectedValue.ToString()));

            Rebind();
        }

        if (e.CommandName.ToUpper().Equals("VESSELLIST"))
        {
        }

        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvAttachment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            UserControlQuick ucAdvancestatus = (UserControlQuick)e.Item.FindControl("ucAdvancestatus");
            if (ucAdvancestatus != null)
            {
                ucAdvancestatus.QuickList = PhoenixRegistersQuick.ListQuick(1, 133);

            }

        }
    }

    protected void Rebind()
    {
        gvAttachment.SelectedIndexes.Clear();
        gvAttachment.EditIndexes.Clear();
        gvAttachment.DataSource = null;
        gvAttachment.Rebind();
    }

    protected void gvAttachment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAttachment.CurrentPageIndex + 1;
        BindData();
    }
}
