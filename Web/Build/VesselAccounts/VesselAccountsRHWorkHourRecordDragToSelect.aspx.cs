using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Generic;
using Telerik.Web.UI;
using SouthNests.Phoenix.PlannedMaintenance;
public partial class VesselAccountsRHWorkHourRecordDragToSelect : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["EMPID"] = null;
                ViewState["RHSTARTID"] = null;
                ViewState["CALENDERID"] = null;
                ViewState["NOOFCOMPLIANCES"] = null;

                PhoenixToolbar toolbar = new PhoenixToolbar();
                //toolbar.AddButton("Close", "CLOSE", ToolBarDirection.Right);
                //toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar.AddButton("Confirm", "CONFIRM", ToolBarDirection.Right);
                toolbar.AddButton("Edit", "EDIT", ToolBarDirection.Right);
                MenuWorkHour.AccessRights = this.ViewState;
                MenuWorkHour.MenuList = toolbar.Show();
                gvMember.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["EDITMODE"] = "0";

                if (Request.QueryString["EMPID"] != null)
                    ViewState["EMPID"] = Request.QueryString["EMPID"].ToString();

                if (Request.QueryString["RHStartId"] != null)
                    ViewState["RHSTARTID"] = Request.QueryString["RHStartId"].ToString();

                if (Request.QueryString["CalenderId"] != null)
                    ViewState["CALENDERID"] = Request.QueryString["CalenderId"].ToString();

                if (Request.QueryString["SHIPCALENDERID"] != null)
                    ViewState["SHIPCALENDERID"] = Request.QueryString["SHIPCALENDERID"].ToString();

                if (Request.QueryString["NOOFCOMPLIANCES"] != null && Request.QueryString["NOOFCOMPLIANCES"].ToString() != "0")
                {
                    ViewState["NOOFCOMPLIANCES"] = Request.QueryString["NOOFCOMPLIANCES"].ToString();
                }

                ViewState["SAVEREQUIRED"] = "1";

                if (Request.QueryString["CalenderId"] != null && Request.QueryString["EMPID"] != null && Request.QueryString["RHStartId"] != null)
                {
                    lnkNatureofwork.Attributes.Add("onclick", "openNewWindow('nature', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsRHNatureOfWork.aspx?CalenderId=" + ViewState["CALENDERID"].ToString() + "&EMPID=" + ViewState["EMPID"].ToString() + "&RHStartId=" + ViewState["RHSTARTID"].ToString() + "'); return false;");
                }

                if (Request.QueryString["MONTHID"] != null)
                    ViewState["MONTHID"] = Request.QueryString["MONTHID"].ToString();

                if (Request.QueryString["YEAR"] != null)
                    ViewState["YEAR"] = Request.QueryString["YEAR"].ToString();

                if (ViewState["CALENDERID"] != null)
                    BindDetails();
            }
            
            lnkReasonNC.Attributes.Add("onclick", "openNewWindow('ncwin', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsRHWorkCalenderRemarks.aspx?CalenderId=" + ViewState["CALENDERID"].ToString() + "&EMPID=" + ViewState["EMPID"].ToString() + "&RHStartId=" + ViewState["RHSTARTID"].ToString() + "'); return true;");
            gvAttandence.Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuWorkHour_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (General.GetNullableString(txtRemarks.Text) == null)
                {
                    ucError.ErrorMessage = "Remarks is required";
                    ucError.Visible = true;
                    return;
                }


                PhoenixVesselAccountsRH.WorkHoursRemarksUpdate(
                    General.GetNullableGuid(ViewState["CALENDERID"].ToString())
                    , int.Parse(ViewState["SHIPCALENDERID"].ToString())
                    , txtRemarks.Text);

                GridTableCell cell =(GridTableCell)gvAttandence.Items[0].Cells[2];

                if (ViewState["SAVEREQUIRED"].ToString() == "1" && cell != null && General.GetNullableGuid(cell.Column.UniqueName) != null)
                {
                    updateworkhourRule(new Guid(cell.Column.UniqueName), int.Parse(ViewState["EMPID"].ToString()));

                    gvAttandence.Rebind();
                    gvWorkHourRecord.Rebind();
                    BindDetails();
                }
                

                ucStatus.Text = "Remarks updated successfully.";
            }
            if (CommandName.ToUpper().Equals("UNLOCK"))
            {
                if (General.GetNullableGuid(ViewState["CALENDERID"].ToString()) != null)
                {
                    PhoenixVesselAccountsRH.WRHUnlockCalendarDays(new Guid(ViewState["CALENDERID"].ToString()));
                    BindDetails();

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('', 'code1', 'Y');", true);
                }

            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                if (string.IsNullOrWhiteSpace(txtRemarks.Text))
                {
                    ucError.ErrorMessage = "Remarks is required";
                    ucError.Visible = true;
                    return;
                }

                RadWindowManager1.RadConfirm("Once Confirmed, you cannot change the work hours. Are you sure you want to confirm?", "ConfirmReconcile", 320, 150, null, "Alert");
                return;
            }
            if (CommandName.ToUpper().Equals("EDIT"))
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Confirm", "CONFIRM", ToolBarDirection.Right);
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuWorkHour.AccessRights = this.ViewState;
                MenuWorkHour.MenuList = toolbar.Show();

                ViewState["EDITMODE"] = 1;
                if (ViewState["EDITMODE"].ToString() == "1")
                {
                    gvAttandence.Enabled = true;
                    gvAttandence.ClientSettings.EnablePostBackOnRowClick = true;
                    gvAttandence.ClientSettings.Selecting.EnableDragToSelectRows = true;
                    gvAttandence.ClientSettings.Selecting.AllowRowSelect = false;
                    gvAttandence.ClientSettings.Selecting.CellSelectionMode = GridCellSelectionMode.MultiCell;
                }

            }

        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }
    private void BindDetails()
    {
        try
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
                ViewState["TOTALHOURS"] = dr["FLDTOTALHOURS"].ToString();
                txtRemarks.Text = dr["FLDWORKHOURSREMARK"].ToString();

                ViewState["SAVEREQUIRED"] = dr["FLDSAVEREQUIRED"].ToString();

                if (General.GetNullableString(dr["FLDNONCOMPLIANCE"].ToString()) == null)
                {
                    lnkReasonNC.Enabled = false;
                    lnkReasonNC.Visible = false;
                }
                else
                {
                    lnkReasonNC.Enabled = true;
                    lnkReasonNC.Visible = true;
                }

                if (dr["FLDISEDIT"].ToString() == "0")
                {
                    PhoenixToolbar toolbar = new PhoenixToolbar();
                    if (dr["FLDUNLOCKENABLEYN"].ToString() == "1")
                        toolbar.AddButton("Unlock", "UNLOCK", ToolBarDirection.Right);

                    MenuWorkHour.AccessRights = this.ViewState;
                    MenuWorkHour.MenuList = toolbar.Show();
                }
                else
                {
                    PhoenixToolbar toolbar = new PhoenixToolbar();
                    //toolbar.AddButton("Close", "CLOSE", ToolBarDirection.Right);
                    //toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    toolbar.AddButton("Confirm", "CONFIRM", ToolBarDirection.Right);
                    toolbar.AddButton("Edit", "EDIT", ToolBarDirection.Right);
                    MenuWorkHour.AccessRights = this.ViewState;
                    MenuWorkHour.MenuList = toolbar.Show();
                }
            }
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
            if (lblhour != null)//SHOWING HOUR TEXT AS '00-01','01-02'......
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
                string s = drv["FLDNONCOMPLIANCE"].ToString();
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
                        uct5.TargetControlId = S5.ClientID;
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
    private void updateworkhourRule(Guid WorkHourID, int empid)
    {
        try
        {
            PhoenixVesselAccountsRH.WorkHourRuleUpdate(WorkHourID, empid);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAttandence_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvAttandence.MasterTableView.Columns.Clear();
        DataSet ds = new DataSet();
        ds = PhoenixVesselAccountsRH.GetWorkHours(new Guid(ViewState["CALENDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (ds.Tables[1].Rows.Count > 0)
        {
            foreach (DataRow drv in ds.Tables[1].Rows)
            {
                GridBoundColumn Item = new GridBoundColumn();
                gvAttandence.MasterTableView.Columns.Add(Item);
                Item.HeaderText = General.GetNullableInteger(drv["FLDREPORTINGHOUR"].ToString()) != null ?
                                        ((int.Parse(drv["FLDREPORTINGHOUR"].ToString()) - 1).ToString("D2") + "-" + (int.Parse(drv["FLDREPORTINGHOUR"].ToString())).ToString("D2")) : drv["FLDREPORTINGHOUR"].ToString();
                Item.UniqueName = drv["FLDRESTHOURWORKID"].ToString();
                Item.HeaderStyle.Width = Unit.Parse("50px");
                Item.ItemStyle.Width = Unit.Parse("50px");
            }

            GridBoundColumn totalHours = new GridBoundColumn();
            gvAttandence.MasterTableView.Columns.Add(totalHours);
            totalHours.HeaderText = "T.H";
            totalHours.UniqueName = "FLDTOTALHOURS";
            totalHours.HeaderStyle.Width = Unit.Parse("50px");
            totalHours.ItemStyle.Width = Unit.Parse("50px");

            GridBoundColumn workHours = new GridBoundColumn();
            gvAttandence.MasterTableView.Columns.Add(workHours);
            workHours.HeaderText = "W.H";
            workHours.UniqueName = "FLDTOTALWORKHOURS";
            workHours.HeaderStyle.Width = Unit.Parse("50px");
            workHours.ItemStyle.Width = Unit.Parse("50px");

            GridBoundColumn restHours = new GridBoundColumn();
            gvAttandence.MasterTableView.Columns.Add(restHours);
            restHours.HeaderText = "R.H";
            restHours.UniqueName = "FLDTOTALRESTHOURS";
            restHours.HeaderStyle.Width = Unit.Parse("50px");
            restHours.ItemStyle.Width = Unit.Parse("50px");
        }

        gvAttandence.DataSource = ds;

        if (ViewState["EDITMODE"].ToString() == "0")
        {
            gvAttandence.Enabled = false;
            gvAttandence.ClientSettings.EnablePostBackOnRowClick = false;
            gvAttandence.ClientSettings.Selecting.EnableDragToSelectRows = true;
            gvAttandence.ClientSettings.Selecting.AllowRowSelect = false;
            gvAttandence.ClientSettings.Selecting.CellSelectionMode = GridCellSelectionMode.None;
        }
        else
        {
            gvAttandence.Focus();
        }

        

    }
    protected void gvAttandence_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

            ViewState["SAVEREQUIRED"] = drv["FLDSAVEREQUIRED"].ToString();
            DataSet ds = (DataSet)((RadGrid)sender).DataSource;



            foreach (GridColumn c in gvAttandence.MasterTableView.Columns)
            {
                if (General.GetNullableGuid(c.UniqueName.ToString()) != null)
                {
                    DataRow[] dr = ds.Tables[1].Select("FLDRESTHOURWORKID = '" + c.UniqueName + "'");

                    decimal hoursatsea = General.GetNullableDecimal(dr[0]["FLDHOURSATSEA"].ToString()) != null ? decimal.Parse(dr[0]["FLDHOURSATSEA"].ToString()) : 0;
                    decimal hoursatport = General.GetNullableDecimal(dr[0]["FLDHOURSATPORT"].ToString()) != null ? decimal.Parse(dr[0]["FLDHOURSATPORT"].ToString()) : 0;
                    if (hoursatsea > 0)
                        item[c.UniqueName].Text = string.Format("{0:0.0}", hoursatsea);
                    else if (hoursatport > 0)
                        item[c.UniqueName].Text = string.Format("{0:0.0}", hoursatport);
                    else
                        item[c.UniqueName].Text = "";

                    if (General.GetNullableString(dr[0]["FLDNONCOMPLIANCE"].ToString()) != null)
                        item[c.UniqueName].BackColor = System.Drawing.Color.Red;
                    else if (hoursatsea > 0 && hoursatsea < 1)
                        item[c.UniqueName].BackColor = System.Drawing.Color.Yellow;
                    else if (hoursatport > 0 && hoursatport < 1)
                        item[c.UniqueName].BackColor = System.Drawing.Color.Yellow;
                    else if (hoursatsea >= 1 || hoursatport >= 1)
                        item[c.UniqueName].BackColor = System.Drawing.Color.Gray;

                }

            }
            decimal totalhours = General.GetNullableDecimal(drv["FLDHOURS"].ToString()) != null ? decimal.Parse(drv["FLDHOURS"].ToString()) : 0;
            decimal totalworkhours = General.GetNullableDecimal(drv["FLDTOTALHOURS"].ToString()) != null ? decimal.Parse(drv["FLDTOTALHOURS"].ToString()) : 0;
            item["FLDTOTALHOURS"].Text = drv["FLDHOURS"].ToString();
            item["FLDTOTALHOURS"].Enabled = false;
            item["FLDTOTALWORKHOURS"].Text = drv["FLDTOTALHOURS"].ToString();
            item["FLDTOTALWORKHOURS"].Enabled = false;
            item["FLDTOTALRESTHOURS"].Text = (totalhours - totalworkhours).ToString();
            item["FLDTOTALRESTHOURS"].Enabled = false;
        }
    }

    protected void lnk05_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridTableCell cell in gvAttandence.SelectedCells)
            {
                GridDataItem item = (GridDataItem)cell.Parent;

                PhoenixVesselAccountsRH.SeaWorkHourUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            new Guid(cell.Column.UniqueName),
                                                            decimal.Parse("0.5"),
                                                            PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                            int.Parse(ViewState["SHIPCALENDERID"].ToString()),
                                                            int.Parse(ViewState["MONTHID"].ToString()),
                                                            int.Parse(ViewState["YEAR"].ToString())
                                                            );
                updateworkhourRule(new Guid(cell.Column.UniqueName), int.Parse(ViewState["EMPID"].ToString()));

            }
            gvAttandence.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void lnk10_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridTableCell cell in gvAttandence.SelectedCells)
            {
                GridDataItem item = (GridDataItem)cell.Parent;

                PhoenixVesselAccountsRH.SeaWorkHourUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            new Guid(cell.Column.UniqueName),
                                                            decimal.Parse("1.0"),
                                                            PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                            int.Parse(ViewState["SHIPCALENDERID"].ToString()),
                                                            int.Parse(ViewState["MONTHID"].ToString()),
                                                            int.Parse(ViewState["YEAR"].ToString())
                                                            );
                updateworkhourRule(new Guid(cell.Column.UniqueName), int.Parse(ViewState["EMPID"].ToString()));

            }
            gvAttandence.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void lnk00_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridTableCell cell in gvAttandence.SelectedCells)
            {
                GridDataItem item = (GridDataItem)cell.Parent;

                PhoenixVesselAccountsRH.SeaWorkHourUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            new Guid(cell.Column.UniqueName),
                                                            decimal.Parse("0.0"),
                                                            PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                            int.Parse(ViewState["SHIPCALENDERID"].ToString()),
                                                            int.Parse(ViewState["MONTHID"].ToString()),
                                                            int.Parse(ViewState["YEAR"].ToString())
                                                            );
                updateworkhourRule(new Guid(cell.Column.UniqueName), int.Parse(ViewState["EMPID"].ToString()));

            }
            gvAttandence.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void ucConfirmReconcile_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixVesselAccountsRH.WorkHoursConfirmation(new Guid(ViewState["CALENDERID"].ToString()), General.GetNullableString(txtRemarks.Text));

            GridTableCell cell = (GridTableCell)gvAttandence.Items[0].Cells[2];

            if (ViewState["SAVEREQUIRED"].ToString() == "1" && cell != null && General.GetNullableGuid(cell.Column.UniqueName) != null)
            {
                updateworkhourRule(new Guid(cell.Column.UniqueName), int.Parse(ViewState["EMPID"].ToString()));

                gvAttandence.Rebind();
                gvWorkHourRecord.Rebind();
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('MoreInfo', 'code1', null);", true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void Record_Click(object sender, EventArgs e)
    //{
    //    foreach (GridTableCell cell in gvAttandence.SelectedCells)
    //    {
    //        GridDataItem item = (GridDataItem)cell.Parent;
    //        decimal hrs= 0.0m ;

    //        if (General.GetNullableDecimal(cell.Text) == null || General.GetNullableDecimal(cell.Text) == 0)
    //            hrs = 1.0m;
    //        else if (General.GetNullableDecimal(cell.Text) != null && General.GetNullableDecimal(cell.Text) < 1)
    //            hrs = 0.0m;
    //        else if (General.GetNullableDecimal(cell.Text) != null && General.GetNullableDecimal(cell.Text) == 1)
    //            hrs = 0.5m;

    //        PhoenixVesselAccountsRH.SeaWorkHourUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //                                                    new Guid(cell.Column.UniqueName),
    //                                                    hrs,
    //                                                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
    //                                                    int.Parse(ViewState["SHIPCALENDERID"].ToString()),
    //                                                     int.Parse(ViewState["MONTHID"].ToString()),
    //                                                     int.Parse(ViewState["YEAR"].ToString())
    //                                                    );
    //        updateworkhourRule(new Guid(cell.Column.UniqueName), int.Parse(ViewState["EMPID"].ToString()));

    //    }
    //}

    protected void gvAttandence_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "RowClick")
            {
                GridDataItem item = (GridDataItem)e.Item;
                foreach (GridTableCell cell in gvAttandence.SelectedCells)
                {
                    if (General.GetNullableGuid(cell.Column.UniqueName) != null)
                    {
                        //GridDataItem item = (GridDataItem)cell.Parent;
                        decimal hrs = 0.0m;

                        if (General.GetNullableDecimal(cell.Text) == null || General.GetNullableDecimal(cell.Text) == 0)
                            hrs = 1.0m;
                        else if (General.GetNullableDecimal(cell.Text) != null && General.GetNullableDecimal(cell.Text) < 1)
                            hrs = 0.0m;
                        else if (General.GetNullableDecimal(cell.Text) != null && General.GetNullableDecimal(cell.Text) == 1)
                            hrs = 0.5m;

                        PhoenixVesselAccountsRH.SeaWorkHourUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    new Guid(cell.Column.UniqueName),
                                                                    hrs,
                                                                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                                    int.Parse(ViewState["SHIPCALENDERID"].ToString()),
                                                                    int.Parse(ViewState["MONTHID"].ToString()),
                                                                    int.Parse(ViewState["YEAR"].ToString())
                                                                    );

                    }

                }
                gvAttandence.Rebind();


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkHourRecord_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvMember_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int IRowcount = 0;
        int ITotalPagecount = 0;

        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.MemberActivitySearch(General.GetNullableGuid(ViewState["CALENDERID"].ToString()), General.GetNullableInteger(ViewState["EMPID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID, gvMember.CurrentPageIndex + 1, gvMember.PageSize, ref IRowcount, ref ITotalPagecount);

        gvMember.DataSource = dt;
        gvMember.VirtualItemCount = IRowcount;

    }

    protected void gvMember_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                LinkButton Timesheet = (LinkButton)item.FindControl("cmdTimesheet");
                RadLabel Memberid = (RadLabel)item.FindControl("lblmemberid");
                RadLabel activityid = (RadLabel)item.FindControl("lblactivityid");
                RadLabel CatId = (RadLabel)item.FindControl("lblcatid");
                RadLabel Useredited = (RadLabel)item.FindControl("lbluseredited");
                LinkButton flag = (LinkButton)item.FindControl("imgFlag");
                RadLabel Type = (RadLabel)item.FindControl("lbltype");
                if (Useredited.Text == "1")
                {
                    flag.Visible = true;

                }
                 if (Timesheet != null)
                {

                    if (CatId.Text == "4")
                    {
                        Timesheet.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','','PlannedMaintenance/PlannedMaintenanceDailyWorkPlanMemberTimeSheet.aspx?Memberid=" + Memberid.Text + "&type=" + Type.Text + "&id=" + activityid.Text +"&readonly=1"+ "','false','700px','350px',null,null);return false");
                        Timesheet.Visible = true;
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
}