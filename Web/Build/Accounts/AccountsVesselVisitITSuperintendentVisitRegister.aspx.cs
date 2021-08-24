using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Reports;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsVesselVisitITSuperintendentVisitRegister : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();       
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsVesselVisitITSuperintendentVisitRegister.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVisit')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsVesselVisitITSuperintendentSearch.aspx'); return false;", "Find", "search.png", "FIND");
            toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsVesselVisitITSuperintendentAdvanceSearch.aspx'); return false;", "Advance Find", "search.png", "ADVANCEFIND");
            toolbargrid.AddImageButton("../Accounts/AccountsVesselVisitITSuperintendentVisitRegister.aspx", "Clear", "clear-filter.png", "CLEAR");
            MenuVisit.AccessRights = this.ViewState;
            MenuVisit.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["FormNo"] = 1;
                ViewState["VisitId"] = null;
                ViewState["VisitType"] = null;
               // ViewState["VisitStatus"] = null;
                ViewState["CurrentVisitType"] = null;

                if (Request.QueryString["VisitId"] != null)
                    ViewState["VisitId"] = Request.QueryString["VisitId"].ToString();
                if (Request.QueryString["VisitType"] != null)
                    ViewState["VisitType"] = Request.QueryString["VisitType"].ToString();
               // if (Request.QueryString["VisitStatus"] != null)
                 //   ViewState["VisitStatus"] = Request.QueryString["VisitStatus"].ToString();


                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvVisit.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindData();
            PhoenixToolbar toolbar = new PhoenixToolbar();

              if (ViewState["CurrentVisitType"] != null && ViewState["CurrentVisitType"].ToString() == "2")
                  toolbar.AddButton("Bill now", "BILLNOW", ToolBarDirection.Right);

            //  if (ViewState["VisitType"] != null && ViewState["VisitType"].ToString() == "1")
             //   toolbar.AddButton("Bill now", "BILLNOW", ToolBarDirection.Right);

            // if ((ViewState["VisitStatus"] != null && ViewState["VisitStatus"].ToString() == "1480") || (ViewState["VisitStatus"] != null &&   ViewState["VisitStatus"].ToString() == "1481"))
            if (Convert.ToString(ViewState["VisitStatus"]) == "1480" || Convert.ToString(ViewState["VisitStatus"]) == "1481")
                toolbar.AddButton("Travel Claims", "CLAIMS", ToolBarDirection.Right);
                toolbar.AddButton("Office/Travel Advance", "ADVANCE", ToolBarDirection.Right);

                if (ViewState["VisitType"] != null && ViewState["VisitType"].ToString() == "1")
                    toolbar.AddButton("Line Item", "LINEITEM", ToolBarDirection.Right);
                toolbar.AddButton("Vessel Visit", "VISIT", ToolBarDirection.Right);

                MenuVisitMain.AccessRights = this.ViewState;
                MenuVisitMain.MenuList = toolbar.Show();
                MenuVisitMain.SelectedMenuIndex = 3;


          //  }


            //toolbar.AddButton("Bill Now", "BILLNOW", ToolBarDirection.Right);

            //if (Convert.ToString(ViewState["VisitStatus"]) == "1480" || Convert.ToString(ViewState["VisitStatus"]) == "1481")
            //    toolbar.AddButton("Travel Claims", "CLAIMS", ToolBarDirection.Right);
            //toolbar.AddButton("Office/Travel Advance", "ADVANCE", ToolBarDirection.Right);

            //if (ViewState["VisitType"] != null && Convert.ToString(ViewState["VisitType"]) == "1")
            //    toolbar.AddButton("Line Item", "LINEITEM", ToolBarDirection.Right);
            //toolbar.AddButton("Vessel Visit", "VISIT", ToolBarDirection.Right);



        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVisitMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("VISIT"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitITSuperintendentVisitRegister.aspx");
            }
            else if (CommandName.ToUpper().Equals("LINEITEM"))
            {
                if (ViewState["VisitId"] != null && ViewState["VisitId"].ToString() != string.Empty)
                {
                    if (ViewState["VisitType"].ToString() == "1")
                        Response.Redirect("../Accounts/AccountsVesselVisitITSuperintendentVisitLineItem.aspx?visitId=" + ViewState["VisitId"].ToString(), false);

                    else
                        MenuVisitMain.SelectedMenuIndex = 3;
                }
            }
            if (CommandName.ToUpper().Equals("ADVANCE"))
            {
                if (ViewState["VisitId"] != null && ViewState["VisitId"].ToString() != string.Empty)
                    Response.Redirect("../Accounts/AccountsVesselVisitTravelAdvanceRequest.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&VisitType=" + ViewState["VisitType"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("CLAIMS"))
            {
                if (ViewState["VisitId"] != null && ViewState["VisitId"].ToString() != string.Empty)
                    Response.Redirect("../Accounts/AccountsVesselVIistTravelClaimRegister.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&VisitType=" + ViewState["VisitType"].ToString() + "&VisitStatus=" + ViewState["VisitStatus"].ToString(), false);//+ "&gvindex=" + gvVisit.SelectedIndexVesselVisitStatus
            }
            if (CommandName.ToUpper().Equals("BILLNOW"))
            {
                // if (ViewState["VisitId"] != null && ViewState["VisitId"].ToString() != string.Empty && Convert.ToString(ViewState["VisitStatus"]) == "1480")                    
                Response.Redirect("../Accounts/AccountsVesselVisitMonthlybilling.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&VisitType=" + ViewState["VisitType"].ToString(), false);
            }
            else
                MenuVisitMain.SelectedMenuIndex = 3;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVisit_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentVesselvisitSelection = null;
                Rebind();
                SetRowSelection();
                BindPageURL(0);

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
        //SetRowSelection();
         Rebind();
        
        if (Session["New"].ToString() == "Y")
        {
            Session["New"] = "N";
            BindPageURL(0);
        }
        if (Session["OPTION"] != null && Session["OPTION"].ToString() == "3")
        {
            Response.Redirect("../Accounts/AccountsVesselVisitTravelAdvanceRequest.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&VisitType=" + ViewState["VisitType"].ToString(), false);
            Session.Remove("OPTION");
        }
    }

    protected void gvVisit_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        try
        {
            if (e.Item is GridDataItem)
            {
                RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
                RadLabel lblClaimStatus = (RadLabel)e.Item.FindControl("lblClaimstatus");


                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTip");

                lblStatus.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
                lblStatus.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");

                if (lblStatus.Text == "Cancelled")
                    uct.Visible = true;
                else
                    uct.Visible = false;

                string visitId = ((RadLabel)e.Item.FindControl("lblVisitId")).Text;
                string formnumber = ((RadLabel)e.Item.FindControl("lblForm")).Text;

                ImageButton noAtt = (ImageButton)e.Item.FindControl("cmdNoAttachment");
                ImageButton att = (ImageButton)e.Item.FindControl("cmdAttachment");
                ImageButton genpdf = (ImageButton)e.Item.FindControl("cmdGenPdf");
                RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
                RadLabel lblclaimamount = (RadLabel)e.Item.FindControl("lblclaimamount");

                string strClaimamount = ((RadLabel)e.Item.FindControl("lblclaimamount")).Text;



                ImageButton imbApprovalHistory = (ImageButton)e.Item.FindControl("imbApprovalHistory");

                ImageButton cmdApprove = (ImageButton)e.Item.FindControl("cmdApprove");
                if (cmdApprove != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName)) cmdApprove.Visible = false;
                }

                if (lblClaimStatus.Text != "PA")
                {
                    cmdApprove.Visible = false;
                }
                else
                {
                    if (strClaimamount != "" && decimal.Parse(strClaimamount) > 100)
                    {
                        e.Item.ForeColor = System.Drawing.Color.Red;
                    }
                }

                if (imbApprovalHistory != null && visitId != null)
                {
                    imbApprovalHistory.Attributes.Add("onclick", "openNewWindow('PaymentVoucherApproval', '', '" + Session["sitepath"] + "/Accounts/AccountsVesselVisitClaimApprovalRevokalHistory.aspx?visitId=" + visitId + "&Formnumber=" + formnumber + "');return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, imbApprovalHistory.CommandName)) imbApprovalHistory.Visible = false;
                }

                if (lblStatus.Text != "Completed")
                    genpdf.Visible = false;

                RadLabel lblEmployeeName = (RadLabel)e.Item.FindControl("lblEmployeeName");
                UserControlToolTip ucToolTipEmpname = (UserControlToolTip)e.Item.FindControl("ucToolTipEmpname");
                if (lblEmployeeName != null && ucToolTipEmpname != null)
                {
                    UserControlToolTip ucta = (UserControlToolTip)e.Item.FindControl("ucToolTipEmpname");
                    ucta.Position = ToolTipPosition.TopCenter;
                    ucta.TargetControlId = lblEmployeeName.ClientID;
                }

                RadLabel lblPICName = (RadLabel)e.Item.FindControl("lblPICName");
                UserControlToolTip ucToolTipPIC = (UserControlToolTip)e.Item.FindControl("ucToolTipPIC");
                if (lblPICName != null && ucToolTipPIC != null)
                {
                    UserControlToolTip uct1 = (UserControlToolTip)e.Item.FindControl("ucToolTipPIC");
                    uct1.Position = ToolTipPosition.TopCenter;
                    uct1.TargetControlId = lblPICName.ClientID;
                }


                if (drv != null && drv["FLDATTACHMENTCOUNT"].ToString() != "0")
                {
                    if (att != null)
                    {
                        att.Visible = true;
                        noAtt.Visible = false;
                        //att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=ACCOUNTS&type=&cmdname=&VESSELID=" + lblVesselId.Text + "'); return true;");
                        att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=ACCOUNTS&mimetype=.pdf&cmdname=&VESSELID=" + lblVesselId.Text + "'); return true;");
                        att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    }
                }
                else
                {

                    if (noAtt != null)
                    {
                        att.Visible = false;
                        noAtt.Visible = true;
                        //noAtt.Attributes.Add("onclick", "javascript:parent.openNewWindow('noAtt','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=ACCOUNTS&type=&cmdname=&VESSELID=" + lblVesselId.Text + "'); return true;");
                        noAtt.Attributes.Add("onclick", "javascript:openNewWindow('noAtt','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=ACCOUNTS&mimetype=.pdf&cmdname=&VESSELID=" + lblVesselId.Text + "'); return true;");
                        noAtt.Visible = SessionUtil.CanAccess(this.ViewState, noAtt.CommandName);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvVisit_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
                ViewState["VisitId"] = ((RadLabel)e.Item.FindControl("lblVisitId")).Text;
                ViewState["VisitType"] = ((RadLabel)e.Item.FindControl("lblVisitType")).Text;
                ViewState["VisitStatus"] = ((RadLabel)e.Item.FindControl("lblvisitstaus")).Text;
                ViewState["CurrentVisitType"] = ((RadLabel)e.Item.FindControl("lblVisitType")).Text;


                if (ViewState["VisitType"] != null && ViewState["VisitType"].ToString() == "1")
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitITVisit.aspx?visitId=" + ViewState["VisitId"].ToString();

                else if (ViewState["VisitType"] != null && ViewState["VisitType"].ToString() == "2")
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitSuperintendentVisit.aspx?visitId=" + ViewState["VisitId"].ToString();

                else if (ViewState["VisitType"] != null && ViewState["VisitType"].ToString() == "3")
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitRidingSuperintendentVisit.aspx?visitId=" + ViewState["VisitId"].ToString();

                else if (ViewState["VisitType"] != null && ViewState["VisitType"].ToString() == "4")
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitOwnerBusinessTravel.aspx?visitId=" + ViewState["VisitId"].ToString();

            }
            else if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                BindPageURL(e.Item.ItemIndex);
            }

            else if (e.CommandName.ToUpper().Equals("NOATTACHMENT"))
            {
                BindPageURL(e.Item.ItemIndex);
            }
            else if (e.CommandName.ToUpper().Equals("GENERATE"))
            {
                GeneratePDF();
            }
            else if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                ViewState["FLDCLAIMAMOUNT"] = ((RadLabel)e.Item.FindControl("lblclaimamount")).Text;

                if (ViewState["FLDCLAIMAMOUNT"] != null && ViewState["FLDCLAIMAMOUNT"].ToString() != string.Empty)
                {
                    int iApprovalStatusAccounts;
                    int? onbehaalf = null;


                    DataTable dt = PhoenixCommonApproval.ListApprovalOnbehalf(1585, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);

                    if (dt.Rows.Count > 0)
                    {
                        onbehaalf = General.GetNullableInteger(dt.Rows[0]["FLDUSERCODE"].ToString());
                    }
                    string Status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 49, "APP");
                    DataTable dt1 = PhoenixCommonApproval.InsertApprovalRecord(ViewState["VisitId"].ToString(), 1585, onbehaalf, int.Parse(Status), ".", PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);
                    iApprovalStatusAccounts = int.Parse(dt1.Rows[0][0].ToString());

                    byte bAllApproved = 0;
                    DataTable dts = PhoenixCommonApproval.ListApprovalRecord(ViewState["VisitId"].ToString(), 1585, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null, 1, ref bAllApproved);

                    PhoenixCommonApproval.Approve((PhoenixModule)Enum.Parse(typeof(PhoenixModule), PhoenixModule.ACCOUNTS.ToString()), 1585, ViewState["VisitId"].ToString(), iApprovalStatusAccounts, bAllApproved == 1 ? true : false, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString());


                    if (iApprovalStatusAccounts.ToString() == "420")
                    {
                        PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                                                    , new Guid(ViewState["VisitId"].ToString()));
                    }

                    ucStatus.Text = "Travel claim approved.";
                    Rebind();
                    RadLabel lblVisitId1 = ((RadLabel)e.Item.FindControl("lblVisitId"));

                    if (lblVisitId1 != null)
                    {
                        BindPageURL(e.Item.ItemIndex);
                        SetRowSelection();
                    }
                }
            }
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
                gvVisit.SelectedIndexes.Clear();
            }

            if (e.CommandName == "ChangePageSize")
            {
                return;
            }
            Rebind();
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
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDEMPLOYEEID", "FLDEMPLOYEEFULLNAME", "FLDFROMDATE", "FLDTODATE", "FLDVISITDAYS", "FLDVESSELNAME", "FLDHARDNAME", "FLDCLAIMSTATUS", "FLDFORMNUMBER", "FLDTYPE", "FLDEXPENSETYPE", "FLDVOUCHERNUMBER", "FLDMONTHLYBILLVOUCHERDATE", "FLDPICFULLNAME" };
            string[] alCaptions = { "Employee Id", "Employee Name", "From Date", "To Date", " No of days", "Vessel Name", "Visit Status", "Claim Status", "Form No", "Type", "Expense Type", "Voucher Number", "Voucher Date", "PIC" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds = new DataSet();

            NameValueCollection nvc = Filter.CurrentVesselvisitSelection;

            if (nvc != null && nvc.Get("FIND") != null && nvc.Get("FIND").ToString().ToUpper().Equals("ADVANCE"))
            {

                ds = PhoenixAccountsVesselVisitITSuperintendentRegister.VisitAdvanceList(
                nvc != null ? General.GetNullableString(nvc.Get("txtEmpId")) : null,
                nvc != null ? General.GetNullableString(nvc.Get("txtEmployeeNameSearch")) : null,
                nvc != null ? General.GetNullableString(nvc.Get("txtFormNo")) : null,
                nvc != null ? General.GetNullableString(nvc.Get("chkVesselList")) : null,
                nvc != null ? General.GetNullableDateTime(nvc.Get("txtstartFromDate")) : null,
                nvc != null ? General.GetNullableDateTime(nvc.Get("txtstartToDate")) : null,
                nvc != null ? General.GetNullableDateTime(nvc.Get("txtendFromDate")) : null,
                nvc != null ? General.GetNullableDateTime(nvc.Get("txtendToDate")) : null,
                nvc != null ? General.GetNullableString(nvc.Get("txtVisitStatus")) : null,
                nvc != null ? General.GetNullableString(nvc.Get("txtclaimStatus")) : null,
                nvc != null ? General.GetNullableString(nvc.Get("ddlVisitTypeSearch")) : null,
                sortexpression, sortdirection,
                gvVisit.CurrentPageIndex+1,
               gvVisit.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                nvc != null ? General.GetNullableInteger(nvc.Get("ddlExpensetype")) : null,
                nvc != null ? General.GetNullableString(nvc.Get("txtPICName")) : null);
            }
            else
            {

                ds = PhoenixAccountsVesselVisitITSuperintendentRegister.VisitList(null,
                  nvc != null ? General.GetNullableString(nvc.Get("txtEmployeeNameSearch")) : null,
                   nvc != null ? General.GetNullableString(nvc.Get("txtFormNo")) : null,
                  nvc != null ? General.GetNullableString(nvc.Get("chkVesselList")) : null,
                  nvc != null ? General.GetNullableDateTime(nvc.Get("txtstartFromDate")) : null,
                  nvc != null ? General.GetNullableDateTime(nvc.Get("txtstartToDate")) : null,
                  nvc != null ? General.GetNullableDateTime(nvc.Get("txtendFromDate")) : null,
                  nvc != null ? General.GetNullableDateTime(nvc.Get("txtendToDate")) : null,
                  nvc != null ? General.GetNullableString(nvc.Get("txtVisitStatus")) : null,
                  nvc != null ? General.GetNullableString(nvc.Get("txtclaimStatus")) : null,
                  nvc != null ? General.GetNullableString(nvc.Get("ddlVisitTypeSearch")) : null,
                  sortexpression, sortdirection,
                       gvVisit.CurrentPageIndex + 1,
               gvVisit.PageSize,
                  ref iRowCount,
                  ref iTotalPageCount,
                  nvc != null ? General.GetNullableInteger(nvc.Get("ddlExpensetype")) : null);
            }

            General.SetPrintOptions("gvVisit", "IT/SuperintendentVisit", alCaptions, alColumns, ds);

            gvVisit.DataSource = ds;
            gvVisit.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                if (ViewState["VisitId"] == null)
                {
                    ViewState["VisitId"] = dr["FLDVISITID"].ToString();
                    ViewState["VisitType"] = dr["FLDVISITTYPE"].ToString();
                    ViewState["VisitStatus"] = dr["FLDVISITSTATUS"].ToString();

                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitITVisit.aspx?visitId=" + ViewState["VisitId"].ToString();
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitITVisit.aspx?visitId=" + ViewState["VisitId"].ToString();
                }

            }
            else
            {
                DataTable dt = ds.Tables[0];
                ViewState["VisitId"] = null;
                ViewState["VisitType"] = null;
                ViewState["VisitStatus"] = null;
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitITVisit.aspx?visitId=";
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDEMPLOYEEID", "FLDEMPLOYEEFULLNAME", "FLDFROMDATE", "FLDTODATE", "FLDVISITDAYS", "FLDVESSELNAME", "FLDHARDNAME", "FLDCLAIMSTATUS", "FLDFORMNUMBER", "FLDTYPE", "FLDEXPENSETYPE", "FLDBUDGETEDVISITTYPE", "FLDVOUCHERNUMBER", "FLDVESSELBUDGETCODE", "FLDMONTHLYBILLVOUCHERDATE", "FLDPICFULLNAME", "FLDREIMBURSEMENTCURRENCY", "FLDCLAIMAMOUNT", "FLDREIMBURSEMENTAMOUNT", "FLDPURPOSE" };
        string[] alCaptions = { "Employee Id", "Employee Name", "From Date", "To Date", " No of days", "Vessel Name", "Visit Status", "Claim Status", "Form No", "Type", "Expense Type","Visit Type", "Voucher Number", "ESM Budget code", "Voucher Date", "PIC", "Reimbursement Currency", "Claim Amount", "Reimbursement Amount", "Purpose" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentVesselvisitSelection;
        if (nvc != null && nvc.Get("FIND").ToString().ToUpper().Equals("ADVANCE"))
        {
            ds = PhoenixAccountsVesselVisitITSuperintendentRegister.VisitAdvanceList(null,
            nvc != null ? General.GetNullableString(nvc.Get("txtEmployeeNameSearch")) : null,
             nvc != null ? General.GetNullableString(nvc.Get("txtFormNo")) : null,
            nvc != null ? General.GetNullableString(nvc.Get("chkVesselList")) : null,
            nvc != null ? General.GetNullableDateTime(nvc.Get("txtstartFromDate")) : null,
            nvc != null ? General.GetNullableDateTime(nvc.Get("txtstartToDate")) : null,
            nvc != null ? General.GetNullableDateTime(nvc.Get("txtendFromDate")) : null,
            nvc != null ? General.GetNullableDateTime(nvc.Get("txtendToDate")) : null,
            nvc != null ? General.GetNullableString(nvc.Get("txtVisitStatus")) : null,
            nvc != null ? General.GetNullableString(nvc.Get("txtclaimStatus")) : null,
            nvc != null ? General.GetNullableString(nvc.Get("ddlVisitTypeSearch")) : null,
            sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
            ref iRowCount,
            ref iTotalPageCount,
            nvc != null ? General.GetNullableInteger(nvc.Get("ddlExpensetype")) : null,
            nvc != null ? General.GetNullableString(nvc.Get("txtPICName")) : null
            );
        }
        else
        {

            ds = PhoenixAccountsVesselVisitITSuperintendentRegister.VisitList(null,
              nvc != null ? General.GetNullableString(nvc.Get("txtEmployeeNameSearch")) : null,
               nvc != null ? General.GetNullableString(nvc.Get("txtFormNo")) : null,
              nvc != null ? General.GetNullableString(nvc.Get("chkVesselList")) : null,
              nvc != null ? General.GetNullableDateTime(nvc.Get("txtstartFromDate")) : null,
              nvc != null ? General.GetNullableDateTime(nvc.Get("txtstartToDate")) : null,
              nvc != null ? General.GetNullableDateTime(nvc.Get("txtendFromDate")) : null,
              nvc != null ? General.GetNullableDateTime(nvc.Get("txtendToDate")) : null,
              nvc != null ? General.GetNullableString(nvc.Get("txtVisitStatus")) : null,
              nvc != null ? General.GetNullableString(nvc.Get("txtclaimStatus")) : null,
              nvc != null ? General.GetNullableString(nvc.Get("ddlVisitTypeSearch")) : null,
              sortexpression, sortdirection,
              (int)ViewState["PAGENUMBER"],
              PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
              ref iRowCount,
              ref iTotalPageCount,
              nvc != null ? General.GetNullableInteger(nvc.Get("ddlExpensetype")) : null);
        }


        Response.AddHeader("Content-Disposition", "attachment; filename= Visit.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>IT/Superintendent Visit</h3></td>");
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

    private void SetRowSelection()
    {
        gvVisit.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvVisit.Items)
        {
            if (item.GetDataKeyValue("FLDVISITID").ToString().Equals(ViewState["VisitId"].ToString()))
            {
                gvVisit.SelectedIndexes.Add(item.ItemIndex);
                ViewState["VisitStatus"] = ((RadLabel)gvVisit.Items[item.ItemIndex].FindControl("lblvisitstaus")).Text;
                ViewState["CurrentVisitType"] = ((RadLabel)gvVisit.Items[item.ItemIndex].FindControl("lblVisitType")).Text;
            }
        }

        if (ViewState["CurrentVisitType"] != null && ViewState["CurrentVisitType"].ToString() == "1")
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitITVisit.aspx?visitId=" + ViewState["VisitId"];

        else if (ViewState["CurrentVisitType"] != null && ViewState["CurrentVisitType"].ToString() == "2")
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitSuperintendentVisit.aspx?visitId=" + ViewState["VisitId"];

        else if (ViewState["CurrentVisitType"] != null && ViewState["CurrentVisitType"].ToString() == "3")
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitRidingSuperintendentVisit.aspx?visitId=" + ViewState["VisitId"];

        else if (ViewState["CurrentVisitType"] != null && ViewState["CurrentVisitType"].ToString() == "4")
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitOwnerBusinessTravel.aspx?visitId=" + ViewState["VisitId"];

    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblVisitId = ((RadLabel)gvVisit.Items[rowindex].FindControl("lblVisitId"));
            RadLabel lblVisitType = ((RadLabel)gvVisit.Items[rowindex].FindControl("lblVisitType"));
            ViewState["VisitType"] = null;

            if (lblVisitId != null)
            {
                ViewState["VisitId"] = ((RadLabel)gvVisit.Items[rowindex].FindControl("lblVisitId")).Text;
                ViewState["VisitStatus"] = ((RadLabel)gvVisit.Items[rowindex].FindControl("lblvisitstaus")).Text;
            }
            if (lblVisitType != null)
            {
                ViewState["VisitType"] = ((RadLabel)gvVisit.Items[rowindex].FindControl("lblVisitType")).Text;
                ViewState["CurrentVisitType"] = ((RadLabel)gvVisit.Items[rowindex].FindControl("lblVisitType")).Text;
            }

            if (ViewState["VisitType"] != null && ViewState["VisitType"].ToString() == "1")
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitITVisit.aspx?visitId=" + ViewState["VisitId"].ToString();

            else if (ViewState["VisitType"] != null && ViewState["VisitType"].ToString() == "2")
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitSuperintendentVisit.aspx?visitId=" + ViewState["VisitId"].ToString();

            else if (ViewState["VisitType"] != null && ViewState["VisitType"].ToString() == "3")
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitRidingSuperintendentVisit.aspx?visitId=" + ViewState["VisitId"].ToString();

            else if (ViewState["VisitType"] != null && ViewState["VisitType"].ToString() == "4")
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsVesselVisitOwnerBusinessTravel.aspx?visitId=" + ViewState["VisitId"].ToString();


            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (ViewState["CurrentVisitType"] != null && ViewState["CurrentVisitType"].ToString() == "2")
                toolbar.AddButton("Bill now", "BILLNOW", ToolBarDirection.Right);

            if ((ViewState["VisitStatus"] != null && ViewState["VisitStatus"].ToString() == "1480") || (ViewState["VisitStatus"] != null && ViewState["VisitStatus"].ToString() == "1481"))
                toolbar.AddButton("Travel Claims", "CLAIMS", ToolBarDirection.Right);
            toolbar.AddButton("Office/Travel Advance", "ADVANCE", ToolBarDirection.Right);

            if (ViewState["VisitType"] != null && ViewState["VisitType"].ToString() == "1")
                toolbar.AddButton("Line Item", "LINEITEM", ToolBarDirection.Right);
            toolbar.AddButton("Vessel Visit", "VISIT", ToolBarDirection.Right);

            MenuVisitMain.AccessRights = this.ViewState;
            MenuVisitMain.MenuList = toolbar.Show();
            MenuVisitMain.SelectedMenuIndex = 3;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void GeneratePDF()
    {
        try
        {

            string[] _reportfile = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            string _filename = "";
            DataSet ds = new DataSet();

            NameValueCollection criteria = new NameValueCollection();

            criteria.Add("applicationcode", "5");
            _reportfile = new string[6];

            _filename = "AccounttravelClimeReport_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf";
            //_filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/AccounttravelClimeReport_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf");
            criteria.Add("reportcode", "TRAVELCLIME");

            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {

                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("applicationcode", "5");
                nvc.Add("reportcode", "TRAVELCLIME");
                nvc.Add("CRITERIA", "");
                string VisitId = (ViewState["VisitId"] == null) ? null : (ViewState["VisitId"].ToString());
                nvc.Add("VisitId", General.GetNullableGuid(VisitId).ToString());
                Session["PHOENIXREPORTPARAMETERS"] = nvc;
                string[] rdlcfilename = new string[11];
                string Tmpfilelocation = string.Empty;
                Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
                Tmpfilelocation = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/TEMP/" + _filename;


                ds = PhoenixReportsCommon.GetReportAndSubReportData(nvc, ref _reportfile, ref _filename);

                PhoenixSSRSReportClass.ExportSSRSReport(_reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, "GenerateSSRSPDF", ref _filename);
                //PhoenixSsrsReportsCommon.GetInterface(rdlcfilename, ds, ExportFileFormat.PDF, "GenerateSSRSPDF", ref _filename);
                Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + _filename + "&type=pdf");
            }
            else
            {
                _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportAccounttravelClimeheadder.rpt");
                _reportfile[1] = "ReportsAccountsTravelAdvance.rpt";
                _reportfile[2] = "ReportAccounttravelClime.rpt";

                string VisitId = (ViewState["VisitId"] == null) ? null : (ViewState["VisitId"].ToString());
                criteria.Add("VisitId", General.GetNullableGuid(VisitId).ToString());
                Session["PHOENIXREPORTPARAMETERS"] = criteria;

                NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
                ds = PhoenixReportsCommon.GetReportAndSubReportData(nvc, ref _reportfile, ref _filename);
                if (ds.Tables.Count > 0)
                {
                    _filename = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/TEMP/AccounttravelClimeReport_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf";
                    //_filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/AccounttravelClimeReport_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf");
                    _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportAccounttravelClimeheadder.rpt");
                    _reportfile[1] = "ReportsAccountsTravelAdvance.rpt";
                    _reportfile[2] = "ReportAccounttravelClime.rpt";

                    // PhoenixReportClass.ExportReportPDF(_reportfile, ref _filename, ds);
                    PhoenixSSRSReportClass.ExportSSRSReport(_reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, "GeneratePDF", ref _filename);
                    Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + _filename + "&type=pdf");
                }
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
        gvVisit.SelectedIndexes.Clear();
        gvVisit.EditIndexes.Clear();
        gvVisit.DataSource = null;
        gvVisit.Rebind();
    }
    protected void gvVisit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVisit.CurrentPageIndex + 1;
        BindData();
    }
}
