using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Generic;
using Telerik.Web.UI;
public partial class VesselAccountsRHWorkCalenderRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuRHGeneral.AccessRights = this.ViewState;
            MenuRHGeneral.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["EMPID"] = null;
                ViewState["RHSTARTID"] = null;
                ViewState["CALENDERID"] = null;

                if (Request.QueryString["EMPID"] != null)
                    ViewState["EMPID"] = Request.QueryString["EMPID"].ToString();

                if (Request.QueryString["CalenderId"] != null)
                    ViewState["CALENDERID"] = Request.QueryString["CalenderId"].ToString();

                if (Request.QueryString["RHStartId"] != null)
                    ViewState["RHSTARTID"] = Request.QueryString["RHStartId"].ToString();

                if (ViewState["CALENDERID"] != null)
                    BindDetails();
                bindChechBox();
                SetDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void bindChechBox()
    {
        try
        {
            chkReason.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 83);
            chkReason.DataBindings.DataValueField = "FLDQUICKCODE";
            chkReason.DataBindings.DataTextField = "FLDQUICKNAME";
            chkReason.DataBind();

            chkCorrectiveAction.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 82);
            chkCorrectiveAction.DataBindings.DataTextField = "FLDQUICKNAME";
            chkCorrectiveAction.DataBindings.DataValueField = "FLDQUICKCODE";
            chkCorrectiveAction.DataBind();

            chkSystemCause.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 86);
            chkSystemCause.DataBindings.DataTextField = "FLDQUICKNAME";
            chkSystemCause.DataBindings.DataValueField = "FLDQUICKCODE";
            chkSystemCause.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RHGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string reason = string.Empty;
                string caction = string.Empty;
                string officereason = string.Empty;
                string syscause = string.Empty;

                foreach (ButtonListItem li in chkReason.Items)
                {
                    reason += (li.Selected ? li.Value + "," : string.Empty);
                }
                reason.TrimEnd(',');

                foreach (ButtonListItem lia in chkCorrectiveAction.Items)
                {
                    caction += (lia.Selected ? lia.Value + "," : string.Empty);
                }
                caction.TrimEnd(',');

                reason = reason + officereason;
                reason.TrimEnd(',');

                foreach (ButtonListItem li in chkSystemCause.Items)
                {
                    syscause += (li.Selected ? li.Value + "," : string.Empty);
                }
                syscause.TrimEnd(',');
                WorkCalenderRemarksSave(reason, caction, syscause);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool ValidReason(string reason, string caction, string natureofwork)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(reason) && string.IsNullOrEmpty(txtremarks.Text))
            ucError.ErrorMessage = "Reason is required.";

        if (string.IsNullOrEmpty(caction))
            ucError.ErrorMessage = "Corrective Action is required.";

        return (!ucError.IsError);
    }
    private void BindDetails()
    {

        DataSet ds = PhoenixVesselAccountsRH.RHWorkCalenderEdit(new Guid(ViewState["CALENDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtEmpName.Text = dr["FLDNAME"].ToString();
            ucRank.SelectedRank = dr["FLDRANKID"].ToString();
            txtDate.Text = string.Format("{0:dd/MMM/yyyy}", dr["FLDDATE"].ToString());
            txtHour.Text = dr["FLDHOURS"].ToString();
            txtReportingday.Text = dr["FLDREPORTINGDAY"].ToString();

            rbnhourchange.SelectedValue = dr["FLDADVANCERETARD"].ToString();
            if (dr["FLDHALFHOURYN"].ToString() != "0" && dr["FLDHALFHOURYN"].ToString() != "")
                rbnhourvalue.SelectedValue = dr["FLDHALFHOURYN"].ToString();
            rbtnadvanceretard.SelectedValue = dr["FLDCLOCK"].ToString();
            rbtnworkplace.SelectedValue = dr["FLDWORKPLACE"].ToString();
        }
    }
    protected void WorkCalenderRemarksSave(string reason, string caction, string syscause)
    {
        try
        {
            PhoenixVesselAccountsRH.UpdateRestHourCalenderRemarks(General.GetNullableGuid(ViewState["CALENDERID"].ToString()),
                                                                    General.GetNullableString(reason),
                                                                    General.GetNullableString(caction),
                                                                    General.GetNullableString(txtremarks.Text),
                                                                    General.GetNullableString(txtCorrectiveRemark.Text),
                                                                    null,
                                                                    General.GetNullableString(syscause),
                                                                    General.GetNullableString(txtSysCause.Text.Trim()));
            SetDetails();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('ncwin', 'code1', null);", true);
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }
    protected void SetDetails()
    {
        DataSet ds = PhoenixVesselAccountsRH.RHWorkCalenderEdit(new Guid(ViewState["CALENDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtremarks.Text = dr["FLDREASONREMARK"].ToString();
            txtCorrectiveRemark.Text = dr["FLDCORRECTIVEREMARK"].ToString();
            txtSysCause.Text = dr["FLDSYSTEMCAUSESREMARK"].ToString();
            string reason = dr["FLDREASON"].ToString();
            string caction = dr["FLDCORRECTIVEACTION"].ToString();
            string syscause = dr["FLDSYSTEMCAUSES"].ToString();

            if (!string.IsNullOrEmpty(reason))
            {
                foreach (string val in reason.Split(','))
                {
                    if (val.Trim() != "")
                    {
                        chkReason.SelectedValue = val;
                    }
                }
            }
            if (!string.IsNullOrEmpty(caction))
            {
                foreach (string val in caction.Split(','))
                {
                    if (val.Trim() != "")
                    {
                        chkCorrectiveAction.SelectedValue = val;
                    }
                }
            }
            if (!string.IsNullOrEmpty(syscause))
            {
                foreach (string val in syscause.Split(','))
                {
                    if (val.Trim() != "")
                    {
                        chkSystemCause.SelectedValue = val;
                    }
                }
            }
        }
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

            DataSet ds = PhoenixVesselAccountsRH.ComplianceList(new Guid(ViewState["CALENDERID"].ToString()), int.Parse(ViewState["EMPID"].ToString()));

            gvWorkHourRecord.DataSource = ds;
            gvWorkHourRecord.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkHourRecord_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblhour = (RadLabel)e.Item.FindControl("lblHour");
            int a = 0;
            if (lblhour != null)
            {
                int.TryParse(lblhour.Text, out a);
                lblhour.Text = (a - 1).ToString().PadLeft(2, '0') + "-" + lblhour.Text.PadLeft(2, '0');
            }

            Image S1 = (Image)e.Item.FindControl("imgS1");
            Image S2 = (Image)e.Item.FindControl("imgS2");
            Image S3 = (Image)e.Item.FindControl("imgS3");
            Image S4 = (Image)e.Item.FindControl("imgS4");
            Image S5 = (Image)e.Item.FindControl("imgS5");
            Image O1 = (Image)e.Item.FindControl("imgO1");
            Image O2 = (Image)e.Item.FindControl("imgO2");
            RadLabel lblexception = (RadLabel)e.Item.FindControl("lblException");
            UserControlToolTip uct1 = (UserControlToolTip)e.Item.FindControl("ucToolTipnc1");
            UserControlToolTip uct2 = (UserControlToolTip)e.Item.FindControl("ucToolTipnc2");
            UserControlToolTip uct3 = (UserControlToolTip)e.Item.FindControl("ucToolTipnc3");
            UserControlToolTip uct4 = (UserControlToolTip)e.Item.FindControl("ucToolTipnc4");
            UserControlToolTip uct5 = (UserControlToolTip)e.Item.FindControl("ucToolTipnc5");
            UserControlToolTip uct6 = (UserControlToolTip)e.Item.FindControl("ucToolTipnc6");
            UserControlToolTip uct7 = (UserControlToolTip)e.Item.FindControl("ucToolTipnc7");

            if (S1 != null && lblexception != null && uct1 != null)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string[] legend = drv["FLDLEGEND"].ToString().TrimEnd(',').Split(',');
                string[] exp = lblexception.Text.ToString().Replace("<BR/>", "#").Split('#');
                List<string> nc = new List<string>();
                List<int> opa = new List<int>();
                for (int c = 0; c < exp.Length; c++)
                    nc.Add(exp[c].ToString());

                for (int i = 0; i < legend.Length; i++)
                {
                    if (legend[i].ToString().ToUpper() == "S1")
                    {
                        S1.ImageUrl = Session["images"] + "/case1.png";
                        uct1.Text = nc[i].ToString();
                        //S1.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct1.ToolTip + "', 'visible');");
                        //S1.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct1.ToolTip + "', 'hidden');");
                        uct1.Position = ToolTipPosition.TopCenter;
                        uct1.TargetControlId = S1.ClientID;
                    }
                    if (legend[i].ToString().ToUpper() == "S2")
                    {
                        S2.ImageUrl = Session["images"] + "/case2.png";
                        uct2.Text = nc[i].ToString();
                        //S2.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct2.ToolTip + "', 'visible');");
                        //S2.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct2.ToolTip + "', 'hidden');");
                        uct2.Position = ToolTipPosition.TopCenter;
                        uct2.TargetControlId = S2.ClientID;
                    }
                    if (legend[i].ToString().ToUpper() == "S3")
                    {
                        S3.ImageUrl = Session["images"] + "/case3.png";
                        uct3.Text = nc[i].ToString();
                        //S3.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct3.ToolTip + "', 'visible');");
                        //S3.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct3.ToolTip + "', 'hidden');");
                        uct3.Position = ToolTipPosition.TopCenter;
                        uct3.TargetControlId = S3.ClientID;
                    }
                    if (legend[i].ToString().ToUpper() == "S4")
                    {
                        S4.ImageUrl = Session["images"] + "/case4.png";
                        uct4.Text = nc[i].ToString();
                        //S4.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct4.ToolTip + "', 'visible');");
                        //S4.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct4.ToolTip + "', 'hidden');");
                        uct4.Position = ToolTipPosition.TopCenter;
                        uct4.TargetControlId = S4.ClientID;
                    }
                    if (legend[i].ToString().ToUpper() == "S5")
                    {
                        S5.ImageUrl = Session["images"] + "/case5.png";
                        uct5.Text = nc[i].ToString();
                        //S5.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct5.ToolTip + "', 'visible');");
                        //S5.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct5.ToolTip + "', 'hidden');");
                        uct5.Position = ToolTipPosition.TopCenter;
                        uct5.TargetControlId = S4.ClientID;
                    }
                    if (legend[i].ToString().ToUpper() == "O1")
                    {
                        O1.ImageUrl = Session["images"] + "/case6.png";
                        uct6.Text = nc[i].ToString();
                        //O1.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct6.ToolTip + "', 'visible');");
                        //O1.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct6.ToolTip + "', 'hidden');");
                        uct6.Position = ToolTipPosition.TopCenter;
                        uct6.TargetControlId = O1.ClientID;
                    }
                    if (legend[i].ToString().ToUpper() == "O2")
                    {
                        O2.ImageUrl = Session["images"] + "/case7.png";
                        uct7.Text = nc[i].ToString();
                        //O2.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct7.ToolTip + "', 'visible');");
                        //O2.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct7.ToolTip + "', 'hidden');");
                        uct7.Position = ToolTipPosition.TopCenter;
                        uct7.TargetControlId = O2.ClientID;
                    }
                }
            }
        }
    }
    protected void gvWorkHourRecord_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvWorkHourRecord_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkHourRecord.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvWorkHourRecord.EditIndexes.Clear();
        gvWorkHourRecord.SelectedIndexes.Clear();
        gvWorkHourRecord.DataSource = null;
        gvWorkHourRecord.Rebind();
    }
}
