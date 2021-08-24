using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsVesselVisitTravelAdvanceRequest : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsVesselVisitTravelAdvanceRequest.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvTravelAdvance')", "Print Grid", "icon_print.png", "PRINT");
            MenuTravelAdvance.AccessRights = this.ViewState;
            MenuTravelAdvance.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();                      
            toolbar1.AddButton("Office/Travel advance", "ADVANCE",ToolBarDirection.Right);
            toolbar1.AddButton("Vessel Visit", "VISIT",ToolBarDirection.Right);

            //if (ViewState["VisitType"].ToString() == "1")
            //    toolbar1.AddButton("Line Item", "LINEITEM");

            MenuTravelAdvanceMain.AccessRights = this.ViewState;
            MenuTravelAdvanceMain.MenuList = toolbar1.Show();

            //if (ViewState["VisitType"] != null && ViewState["VisitType"].ToString() == "1" && ViewState["VisitType"].ToString() == "2")
            //    MenuTravelAdvanceMain.SelectedMenuIndex = 0;
            //else
            //    MenuTravelAdvanceMain.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["VisitId"] = Request.QueryString["VisitId"];
                ViewState["VisitType"] = Request.QueryString["VisitType"];
                ViewState["ClaimStatus"] = null;
                ViewState["VesselVisitStatus"] = "";
                ViewState["APPROVE"] = "";

                BindMainData();
              
                ViewState["PAGENUMBER"] = 1;              
            }
            
            if (ViewState["VesselVisitStatus"].ToString() != "")
            {
                if (ViewState["VesselVisitStatus"].ToString() == "1480" || ViewState["VesselVisitStatus"].ToString() == "1515" || ViewState["VesselVisitStatus"].ToString() == "1514")
                {
                    PhoenixToolbar toolbar = new PhoenixToolbar();

                    if (ViewState["VesselVisitStatus"].ToString() == "1480")
                        toolbar.AddButton("Travel Claims", "CLAIMS",ToolBarDirection.Right);
                    if (ViewState["VisitType"].ToString() == "1")
                        toolbar.AddButton("Line Item", "LINEITEM", ToolBarDirection.Right);
                    toolbar.AddButton("Office/Travel advance", "ADVANCE",ToolBarDirection.Right);
                   
                    ViewState["APPROVE"] = "HIDE";

                    toolbar.AddButton("Vessel Visit", "VISIT",ToolBarDirection.Right);

                    MenuTravelAdvanceMain.AccessRights = this.ViewState;
                    MenuTravelAdvanceMain.MenuList = toolbar.Show();

                    //if (ViewState["VisitType"] != null && ViewState["VisitType"].ToString() == "1" && ViewState["VisitType"].ToString() == "2")
                    //    MenuTravelAdvanceMain.SelectedMenuIndex = 0;
                    //else
                    //    MenuTravelAdvanceMain.SelectedMenuIndex = 1;
                }
                else
                {
                    ViewState["APPROVE"] = "SHOW";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTravelAdvanceMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("VISIT"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitITSuperintendentVisitRegister.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&VisitType=" + ViewState["VisitType"] + "&VisitStatus=" + ViewState["VesselVisitStatus"].ToString());
            }
            else if (CommandName.ToUpper().Equals("LINEITEM"))
            {
                if (ViewState["VisitType"].ToString() == "1")
                    Response.Redirect("../Accounts/AccountsVesselVisitITSuperintendentVisitLineItem.aspx?visitId=" + ViewState["VisitId"].ToString() + "&VisitStatus=" + ViewState["VesselVisitStatus"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("CLAIMS"))
            {
                Response.Redirect("../Accounts/AccountsVesselVIistTravelClaimRegister.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&VisitType=" + ViewState["VisitType"].ToString() + "&VisitStatus=" + ViewState["VesselVisitStatus"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTravelAdvanceSub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["VisitId"].ToString()));
                ucStatus.Text = "Travel advance approved";
                gvTravelAdvance.Rebind();
            }

            if (CommandName.ToUpper().Equals("APPROVAL"))
            {

                //String scriptpopup = String.Format(
                //       "javascript:parent.Openpopup('codehelp1', '', '../Accounts/AccountsVesselVisitSubmitForApprovalConfirmation.aspx?visitId=" + ViewState["VisitId"].ToString() + "','medium');");
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

                if (ViewState["Advexists"].ToString() == "1")
                {
                    PhoenixAccountsVesselVisitITSuperintendentRegister.VisitSubmitForApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["VisitId"].ToString()));
                    String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                }
                else
                {
                    String scriptpopup = String.Format(
                       "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsVesselVisitSubmitForApprovalConfirmation.aspx?visitId=" + ViewState["VisitId"].ToString() + "','large');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                gvTravelAdvance.Rebind();
                BindMainData();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTravelAdvance_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvTravelAdvance.Rebind();
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvTravelAdvance.Rebind();
    }

    protected void gvTravelAdvance_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd")).SelectedCurrency.ToString(),
                    ((UserControlMaskNumber)e.Item.FindControl("txtRequestedAmountAdd")).Text,
                    ((RadComboBox)e.Item.FindControl("ddlAdvance")).SelectedValue.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceRequestInsert(new Guid(ViewState["VisitId"].ToString()),
                    int.Parse(((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd")).SelectedCurrency.ToString()),
                    Convert.ToDecimal(((UserControlMaskNumber)e.Item.FindControl("txtRequestedAmountAdd")).Text),
                    ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text,
                    PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(((RadComboBox)e.Item.FindControl("ddlAdvance")).SelectedValue.ToString())) ;
                gvTravelAdvance.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

                if (!IsValidData(((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit")).SelectedCurrency.ToString(),
                    ((UserControlMaskNumber)e.Item.FindControl("txtRequestedAmountEdit")).Text,""))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceRequestUpdate(
                    new Guid(((RadLabel)e.Item.FindControl("lblTravelAdvanceId")).Text),
                    new Guid(ViewState["VisitId"].ToString()),
                   int.Parse(((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit")).SelectedCurrency.ToString()),
                   Convert.ToDecimal(((UserControlMaskNumber)e.Item.FindControl("txtRequestedAmountEdit")).Text),
                   ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text,
                   PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                
                gvTravelAdvance.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("REJECT"))
            {
                PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceRequestCancel(
                   new Guid(((RadLabel)e.Item.FindControl("lblTravelAdvanceId")).Text),
                   new Guid(ViewState["VisitId"].ToString()),
                  PhoenixSecurityContext.CurrentSecurityContext.UserCode,"Crew");
                ucStatus.Text = "Travel Advance Cancelled";
                gvTravelAdvance.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("TAKEVESSELCASH"))
            {
                PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceRequestTakeVesselCash(
                   new Guid(((RadLabel)e.Item.FindControl("lblTravelAdvanceId")).Text),
                   new Guid(ViewState["VisitId"].ToString()),
                  PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ucStatus.Text = "Taken amount changed";
                gvTravelAdvance.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelAdvance_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton cmdEditCancel = (ImageButton)e.Item.FindControl("cmdEditCancel");
            if (cmdEditCancel != null) cmdEditCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdEditCancel.CommandName);

            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            ImageButton cmdSave = (ImageButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            ImageButton cmdCancel = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cmdCancel != null) cmdCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName);

            ImageButton cmdTakeVesselCash = (ImageButton)e.Item.FindControl("cmdTakeVesselCash");
            RadLabel lblAdvanceStatusCode = (RadLabel)e.Item.FindControl("lblAdvanceStatusCode");
            if (lblAdvanceStatusCode != null && lblAdvanceStatusCode.Text == "RQT")
                
                if (lblAdvanceStatusCode != null && (lblAdvanceStatusCode.Text == "RQT" || lblAdvanceStatusCode.Text == "APP"))
                {
                    if (cmdCancel != null) cmdCancel.Visible = true;
                }
                else
                {
                    if (cmdCancel != null) cmdCancel.Visible = false;
                }

            if (ViewState["ClaimStatus"] != null && ViewState["ClaimStatus"].ToString() == "CPD")
            {
                if (cmdTakeVesselCash != null) cmdTakeVesselCash.Visible = false;
            }
            else
            {
                if (cmdTakeVesselCash != null) cmdTakeVesselCash.Visible = true;
            }
            if (lblAdvanceStatusCode != null && (lblAdvanceStatusCode.Text == "RQT"))
            {
                if (cmdEdit != null) cmdEdit.Visible = true;
            }
            else
            {
                if (cmdEdit != null) cmdEdit.Visible = false;
            }
        }
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblCurrencyId = (RadLabel)e.Item.FindControl("lblCurrencyId");
            if (lblCurrencyId != null && lblCurrencyId.Text != "")
            {
                UserControlCurrency uc = ((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit"));
                uc.SelectedCurrency = lblCurrencyId.Text.ToString();
            }
            RadLabel lblBalance = (RadLabel)e.Item.FindControl("lblBalance");
            if (lblBalance != null && lblBalance.Text != "" && Convert.ToDecimal(lblBalance.Text) < 0)
            {
                lblBalance.Text = "(" + (Convert.ToDecimal(lblBalance.Text) * (-1)).ToString() + ")";
                e.Item.Cells[6].ForeColor = System.Drawing.Color.Red;
            }
        }
        if (e.Item is GridFooterItem)
        {
             UserControlCurrency uc = ((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd"));
             uc.ActiveCurrency = true;
            ImageButton cmdAdd = (ImageButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName))
                    cmdAdd.Visible = false;
            }
            if (ViewState["ClaimStatus"] != null && ViewState["ClaimStatus"].ToString() == "CPD")
            {
                if (cmdAdd != null) cmdAdd.Visible = false;
            }
            else
            {
                if (cmdAdd != null) cmdAdd.Visible = true;
            }
            //if (General.GetNullableString(Convert.ToString(ViewState["VesselVisitStatus"])) == "1480")
            //{
            //    if (cmdAdd != null) cmdAdd.Visible = false;
            //}
            //else
            //{
            //    if (cmdAdd != null) cmdAdd.Visible = true;
            //}
        }
    }

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDTRAVELADVANCENUMBER", "FLDREQUESTEDDATE", "FLDCURRENCYCODE", "FLDREQUESTAMOUNT", "FLDTAKENAMOUNT", "FLDRETURNAMOUNT", "FLDBALANCE", "FLDQUICKNAME", "FLDCLAIMSUBMITTED", "FLDREMARKS" };
            string[] alCaptions = { "Travel Advance Number", "Requested Date", "Currency", "Requested Amount", "Taken Amount", "Return Amount", "Balance", "Advance Status", "Claim Submitted", "Remarks" };

            DataSet ds = new DataSet();

            ds = PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceRequestList(new Guid(ViewState["VisitId"].ToString()),
                                                                              (int)ViewState["PAGENUMBER"],
                                                                              General.ShowRecords(null),
                                                                              ref iRowCount,
                                                                              ref iTotalPageCount);

            General.SetPrintOptions("gvTravelAdvance", "Travel Advance Request", alCaptions, alColumns, ds);

                gvTravelAdvance.DataSource = ds;
            gvTravelAdvance.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            PhoenixToolbar toolbarsub = new PhoenixToolbar();   
            if (ds.Tables[1].Rows.Count > 0 && ViewState["APPROVE"].ToString() == "SHOW")
            {
                toolbarsub.AddButton("Approve", "APPROVE",ToolBarDirection.Right);
                MenuTravelAdvanceSub.AccessRights = this.ViewState;
                MenuTravelAdvanceSub.MenuList = toolbarsub.Show();
                // MenuTravelAdvanceSub.Visible = true;
            }
            else if (ViewState["ShowApproval"] != null && ViewState["ShowApproval"].ToString() == "1")
            {
                toolbarsub.AddButton("Submit for Approval", "APPROVAL",ToolBarDirection.Right);
                MenuTravelAdvanceSub.AccessRights = this.ViewState;
                MenuTravelAdvanceSub.MenuList = toolbarsub.Show();
                // MenuTravelAdvanceSub.Visible = false;
            }
            else
            {
                MenuTravelAdvanceSub.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindMainData()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixAccountsVesselVisitITSuperintendentRegister.VisitSearch(new Guid(ViewState["VisitId"].ToString()));
            DataRow dr = ds.Tables[0].Rows[0];

            txtEmployee.Text = dr["FLDEMPLOYEECODE"].ToString() + " / " + dr["FLDEMPLOYEENAME"].ToString();
            txtVessel.Text = dr["FLDVESSELNAME"].ToString();
            txtPurpose.Text = dr["FLDPURPOSE"].ToString();
            txtFleet.Text = dr["FLDFLEETDESCRIPTION"].ToString();
            ViewState["ClaimStatus"] = dr["FLDCLAIMSTATUS"].ToString();
            ViewState["ShowApproval"] = int.Parse(dr["FLDSHOWAPPROVAL"].ToString());
            ViewState["VesselVisitStatus"] = dr["FLDVISITSTATUS"].ToString();
            ViewState["Advexists"] = dr["FLDADVANCEEXISTS"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidData(string currencyCode, string RequestAmount, string Advfrom)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (currencyCode.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Currency code is required.";

        if (RequestAmount.Trim().Equals(""))
            ucError.ErrorMessage = "Request amount is required.";
        if(Advfrom.Trim().Equals("dummy"))
            ucError.ErrorMessage = "Please chose the Advance from office/vessel cash";

        return (!ucError.IsError);

    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDTRAVELADVANCENUMBER", "FLDREQUESTEDDATE", "FLDCURRENCYCODE", "FLDREQUESTAMOUNT", "FLDTAKENAMOUNT", "FLDRETURNAMOUNT", "FLDBALANCE", "FLDQUICKNAME", "FLDCLAIMSUBMITTED", "FLDREMARKS" };
        string[] alCaptions = { "Travel Advance Number", "Requested Date", "Currency", "Requested Amount", "Taken Amount", "Return Amount", "Balance", "Advance Status", "Claim Submitted", "Remarks" };


        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsVesselVisitTravelAdvance.TravelAdvanceRequestList(new Guid(ViewState["VisitId"].ToString()),
                                                                              (int)ViewState["PAGENUMBER"],
                                                                              iRowCount,
                                                                              ref iRowCount,
                                                                              ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=TravelAdvanceRequest.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Advance Request</h3></td>");
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

    protected void gvTravelAdvance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
