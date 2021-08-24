using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Data;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewReportPrincipleInteraction : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
            PrincipleInteraction.AccessRights = this.ViewState;
            PrincipleInteraction.MenuList = toolbar.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportPrincipleInteraction.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvInteractionList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:Openpopup('Filter','','CrewReportPrincipleInteractionFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportPrincipleInteraction.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuInteractionList.AccessRights = this.ViewState;
            MenuInteractionList.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["INTERTACTIONID"] = string.Empty;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                cblVessel.DataSource = PhoenixRegistersVessel.VesselListCommon(General.GetNullableByte("1")
                                                                               , null
                                                                               , null
                                                                               , General.GetNullableByte("1")
                                                                               , SouthNests.Phoenix.Common.PhoenixVesselEntityType.VSL
                                                                               , null);
                cblVessel.DataTextField = "FLDVESSELNAME";
                cblVessel.DataValueField = "FLDVESSELID";
                cblVessel.DataBind();
                gvInteractionList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvInteractionList.SelectedIndexes.Clear();
        gvInteractionList.EditIndexes.Clear();
        gvInteractionList.DataSource = null;
        gvInteractionList.Rebind();
    }
    protected void MenuInteractionList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucPrincipal.SelectedAddress = "";
                ddlApproval.SelectedValue = "";
                cblVessel.SelectedIndex = -1;
                txtContactDetails.Text = "";
                txtBriefingReq.Text = "";
                txtMonthlyReporting.Text = "";
                txtOtherReq.Text = "";
                Filter.CurrentPrincipleInteractionFilterCriteria = null;
                Rebind();

            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDROWNUMBER", "FLDPRINCIPLENAME", "FLDVESSELSHORTCODE", "FLDAPPROVALYN", "FLDCONTACTDETAILS", "FLDBRIEFINGREQUIRED", "FLDMONTHLYREPORTING", "FLDOTHER", "FLDMODIFIEDBY", "FLDMODIFIEDDATE" };
                string[] alCaptions = { "Sr No", "Principle", "Vessels", "Approval", "Contact", "Briefing", "Monthly Reporting", "Any other Specific req", "Last edited by", "Date" };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewPrincipleInteraction.CrewPrincipleInteractionSearch(null, null, sortexpression, sortdirection
                                                                        , 1, iRowCount
                                                                        , ref iRowCount, ref iTotalPageCount);


                if (ds.Tables.Count > 0)
                    General.ShowExcel("Principle Interaction Details", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDPRINCIPLENAME", "FLDVESSELSHORTCODE", "FLDAPPROVALYN", "FLDCONTACTDETAILS", "FLDBRIEFINGREQUIRED", "FLDMONTHLYREPORTING", "FLDOTHER", "FLDMODIFIEDBY", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Sr No", "Principle", "Vessels", "Approval", "Contact", "Briefing", "Monthly Reporting", "Any other Specific req", "Last edited by", "Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 1; //defaulting descending order for Relief due date
        try
        {
            DataSet ds;
            NameValueCollection nvc = Filter.CurrentPrincipleInteractionFilterCriteria;

            ds = PhoenixCrewPrincipleInteraction.CrewPrincipleInteractionSearch(nvc != null ? General.GetNullableInteger(nvc.Get("ucPrincipal")) : null, nvc != null ? nvc.Get("cblVessel") : null, sortexpression, sortdirection
                                                                    , 1, gvInteractionList.PageSize
                                                                    , ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvInteractionList", "Interation", alCaptions, alColumns, ds);

            gvInteractionList.DataSource = ds;
            gvInteractionList.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PrincipleInteraction_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                StringBuilder strvessel = new StringBuilder();
                foreach (ListItem item in cblVessel.Items)
                {
                    if (item.Selected == true)
                    {
                        strvessel.Append(item.Value.ToString());
                        strvessel.Append(",");
                    }
                }

                if (strvessel.Length > 1)
                {
                    strvessel.Remove(strvessel.Length - 1, 1);
                }

                if (!IsValidate(ucPrincipal.SelectedAddress, strvessel.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }

                if (string.IsNullOrEmpty(ViewState["INTERTACTIONID"].ToString()))
                {
                    PhoenixCrewPrincipleInteraction.InsertCrewPrincipleInteraction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt64(ucPrincipal.SelectedAddress),
                        strvessel.ToString(),
                        General.GetNullableInteger(ddlApproval.SelectedValue),
                        General.GetNullableString(txtContactDetails.Text),
                        General.GetNullableString(txtBriefingReq.Text),
                        General.GetNullableString(txtMonthlyReporting.Text),
                        General.GetNullableString(txtOtherReq.Text));

                    ucStatus.Text = "Information Inserted Successfully";

                    ResetFormControlValues(this);
                    foreach (ListItem vessel in cblVessel.Items)
                        vessel.Selected = false;
                    ViewState["INTERTACTIONID"] = string.Empty;
                }
                else
                {
                    PhoenixCrewPrincipleInteraction.UpdateCrewPrincipleInteraction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Convert.ToInt32(ViewState["INTERTACTIONID"]),
                    Convert.ToInt64(ucPrincipal.SelectedAddress),
                    strvessel.ToString(),
                    General.GetNullableInteger(ddlApproval.SelectedValue),
                    General.GetNullableString(txtContactDetails.Text),
                    General.GetNullableString(txtBriefingReq.Text),
                    General.GetNullableString(txtMonthlyReporting.Text),
                    General.GetNullableString(txtOtherReq.Text));

                    ucStatus.Text = "Information Inserted Successfully";
                    SetInteractionDetails(Convert.ToInt32(ViewState["INTERTACTIONID"]));
                }
                Rebind();

            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {

                ResetFormControlValues(this);
                ViewState["INTERTACTIONID"] = string.Empty;
                gvInteractionList.SelectedIndexes.Clear();
                gvInteractionList.EditIndexes.Clear();
                ucPrincipal.SelectedAddress = "";
                foreach (ListItem vessel in cblVessel.Items)
                    vessel.Selected = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidate(string principle, string vessel)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(principle) == null)
        {
            ucError.ErrorMessage = "Principle is required.";
        }
        if (vessel.Trim() == "")
        {
            ucError.ErrorMessage = "Select atleast one vessel";
        }

        return (!ucError.IsError);
    }

    protected void ucPrincipal_TextChangedEvent(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucPrincipal.SelectedAddress) != null)
        {
            cblVessel.DataSource = PhoenixRegistersVessel.ListCommonVessels(null, null, null, null, 1, ucPrincipal.SelectedAddress, null, null, 1, "VSL");
            cblVessel.DataTextField = "FLDVESSELNAME";
            cblVessel.DataValueField = "FLDVESSELID";
            cblVessel.DataBind();
        }
        else
        {
            cblVessel.DataSource = PhoenixRegistersVessel.ListCommonVessels(null, null, null, null, 1, null, null, null, 1, "VSL");
            cblVessel.DataTextField = "FLDVESSELNAME";
            cblVessel.DataValueField = "FLDVESSELID";
            cblVessel.DataBind();
        }

    }
    protected void gvInteractionList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {


            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null)
            {
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }



            RadLabel lblcontact = (RadLabel)e.Item.FindControl("lblContactDetail");
            UserControlToolTip uctcontact = (UserControlToolTip)e.Item.FindControl("ucToolTipContactDetail");
            lblcontact.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctcontact.ToolTip + "', 'visible');");
            lblcontact.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctcontact.ToolTip + "', 'hidden');");

            RadLabel lblbriefing = (RadLabel)e.Item.FindControl("lblBriefing");
            UserControlToolTip uctbriefing = (UserControlToolTip)e.Item.FindControl("ucToolTipBriefing");
            lblbriefing.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctbriefing.ToolTip + "', 'visible');");
            lblbriefing.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctbriefing.ToolTip + "', 'hidden');");

            RadLabel lblmonthlytreporting = (RadLabel)e.Item.FindControl("lblMonthlyReporting");
            UserControlToolTip uctmonthlytreporting = (UserControlToolTip)e.Item.FindControl("ucToolTipMonthlyReporting");
            lblmonthlytreporting.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctmonthlytreporting.ToolTip + "', 'visible');");
            lblmonthlytreporting.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctmonthlytreporting.ToolTip + "', 'hidden');");

            RadLabel lblOther = (RadLabel)e.Item.FindControl("lblOther");
            UserControlToolTip uctother = (UserControlToolTip)e.Item.FindControl("ucToolTipOther");
            lblOther.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctother.ToolTip + "', 'visible');");
            lblOther.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctother.ToolTip + "', 'hidden');");

        }
    }

    private void SetInteractionDetails(int interactionid)
    {
        DataTable dt = PhoenixCrewPrincipleInteraction.EditCrewPrincipleInteraction(interactionid);
        if (dt.Rows.Count > 0)
        {
            ucPrincipal.SelectedAddress = dt.Rows[0]["FLDPRINCIPLEID"].ToString();
            cblVessel.DataSource = PhoenixRegistersVessel.ListCommonVessels(null, null, null, null, 1, dt.Rows[0]["FLDPRINCIPLEID"].ToString(), null, null, 1, "VSL");
            cblVessel.DataTextField = "FLDVESSELNAME";
            cblVessel.DataValueField = "FLDVESSELID";
            cblVessel.DataBind();
            string[] vessel = dt.Rows[0]["FLDVESSEL"].ToString().Split(',');
            foreach (string item in vessel)
            {
                if (item.Trim() != "")
                {
                    if (cblVessel.Items.FindByValue(item) != null)
                        cblVessel.Items.FindByValue(item).Selected = true;
                }
            }
            ddlApproval.SelectedValue = dt.Rows[0]["FLDAPPROVALYN"].ToString();
            txtContactDetails.Text = dt.Rows[0]["FLDCONTACTDETAILS"].ToString();
            txtBriefingReq.Text = dt.Rows[0]["FLDBRIEFINGREQUIRED"].ToString();
            txtMonthlyReporting.Text = dt.Rows[0]["FLDMONTHLYREPORTING"].ToString();
            txtOtherReq.Text = dt.Rows[0]["FLDOTHER"].ToString();
            ViewState["INTERTACTIONID"] = dt.Rows[0]["FLDINTERACTIONIID"].ToString();
        }
    }


    protected void gvInteractionList_DeleteCommand(object sender, GridCommandEventArgs e)
    {

        string strinteractionid = ((RadLabel)e.Item.FindControl("lblInteractionId")).Text;
        PhoenixCrewPrincipleInteraction.DeleteCrewPrincipleInteraction(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(strinteractionid));
        Rebind();
    }



    private void ResetFormControlValues(Control parent)
    {
        try
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    ResetFormControlValues(c);
                }
                else
                {
                    switch (c.GetType().ToString())
                    {
                        case "Telerik.Web.UI.RadTextBox":
                            ((RadTextBox)c).Text = "";
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((CheckBox)c).Checked = false;
                            break;
                        case "Telerik.Web.UI.RadRadioButton":
                            ((RadRadioButton)c).Checked = false;
                            break;
                        case "Telerik.Web.UI.RadComboBoxItem":
                            ((RadComboBoxItem)c).Selected = false;
                            break;
                        case "UserControlAddressType":
                            ((UserControlAddressType)c).SelectedAddress = "";
                            break;

                        case "Telerik.Web.UI.RadListBox":
                            ((ListBox)c).SelectedIndex = 0;
                            break;

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

    protected void gvInteractionList_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvInteractionList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
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
    protected void gvInteractionList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInteractionList.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvInteractionList_EditCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid _gridView = new RadGrid();
        int nCurrentRow = e.Item.DataSetIndex;
        GridDataItem dataItem = (GridDataItem)e.Item;
        RadLabel lblInteractionId = (RadLabel)dataItem.FindControl("lblInteractionId");
        string strinteractionid = lblInteractionId.Text;
        SetInteractionDetails(int.Parse(strinteractionid));
        ViewState["INTERACTIONID"] = strinteractionid;
    }
}
