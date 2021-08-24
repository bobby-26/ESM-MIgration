using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOperation;
using System.Data;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewWorkGearNeededItem : PhoenixBasePage
{

    private string empid = string.Empty;
    private string vslid = string.Empty;
    private string crewplanid = null;
    private string orderid = null;
    private string r = null;
    private string Neededid = string.Empty;
    string rnkid = string.Empty;
    private string Needed = string.Empty;
    private string newreqid = null;
    private string Orderback = null;
    


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"] != "")
                ViewState["planid"] = Request.QueryString["crewplanid"];
            if (Request.QueryString["vslid"] != null && Request.QueryString["vslid"] != "")
                ViewState["vslid"] = Request.QueryString["vslid"];
            if (Request.QueryString["empid"] != null && Request.QueryString["empid"] != "")
                ViewState["empid"] = Request.QueryString["empid"];
            if (Request.QueryString["Needed"] != null && Request.QueryString["Needed"] != "")
                ViewState["Needed"] = Request.QueryString["Needed"];
            if (Request.QueryString["Neededid"] != null && Request.QueryString["Neededid"] != "")
                ViewState["NEEDEDID"] = Request.QueryString["Neededid"];
            if (Request.QueryString["orderid"] != null && Request.QueryString["orderid"] != "")
                ViewState["ORDERID"] = Request.QueryString["orderid"];
            if (Request.QueryString["r"] != null && Request.QueryString["r"] != "")
                ViewState["r"] = Request.QueryString["r"];
            if (Request.QueryString["newreqid"] != null && Request.QueryString["newreqid"] != "")
                ViewState["newreqid"] = Request.QueryString["newreqid"];
            if (Request.QueryString["Orderback"] != null && Request.QueryString["Orderback"] != "")
                ViewState["Orderback"] = Request.QueryString["Orderback"];


            if (ViewState["empid"] != null)
                empid = ViewState["empid"].ToString();
            if (ViewState["vslid"] != null)
                vslid = ViewState["vslid"].ToString();
            if (ViewState["planid"] != null)
                crewplanid = ViewState["planid"].ToString();
            if (ViewState["Needed"] != null)
                Needed = ViewState["Needed"].ToString();
            if (ViewState["NEEDEDID"] != null)
                Neededid = ViewState["NEEDEDID"].ToString();
            if (ViewState["ORDERID"] != null)
                orderid = ViewState["ORDERID"].ToString();
            if (ViewState["r"] != null)
                r = ViewState["r"].ToString();
            if (ViewState["newreqid"] != null)
                newreqid = ViewState["newreqid"].ToString();
            if (ViewState["Orderback"] != null)
                Orderback = ViewState["Orderback"].ToString();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewWorkGearNeededItem.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvRegistersworkinggearitem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewWorkingGearItemSelection.aspx?back=yes&vesslid=" + vslid + "&empid=" + empid + "&rankid=" + rnkid + "&crewplanid=" + crewplanid + "&Neededid=" + Neededid + "&r=" + r + "&Orderback=" + Orderback + "',false,800,600); return false;", "Add New Requisition", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewWorkGearNeededItem.aspx", "Issue all", "<i class=\"fas fa-check-circle-ei\"></i>", "ISSUEALL");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewWorkGearNeededItem.aspx", "Confirm", "<i class=\"fas fa-award\"></i>", "CONFIRM");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('NAFA', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=WORKGEARISSUEDITEM&Neededid=" + Neededid + "&showmenu=1');return false;", "Show pdf", "<i class=\"fas fa-file-pdf\"></i>", "REPORT");

            Menuitems.AccessRights = this.ViewState;
            Menuitems.MenuList = toolbargrid.Show();
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["MPAGENUMBER"] = 1;
                ViewState["MSORTEXPRESSION"] = null;
                ViewState["MSORTDIRECTION"] = null;
                ViewState["ACTIVE"] = null;
                ViewState["NEEDEDREQID"] = "";

                ucVessel.DataSource = PhoenixRegistersVessel.VesselListCommon(General.GetNullableByte("0")
                                                                             , General.GetNullableByte("1")
                                                                             , null
                                                                             , General.GetNullableByte("1")
                                                                             //  , SouthNests.Phoenix.Common.PhoenixVesselEntityType.VSL
                                                                             , null
                                                                             , null);
                ucVessel.DataTextField = "FLDVESSELNAME";
                ucVessel.DataValueField = "FLDVESSELID";
                ucVessel.DataBind();

                if (vslid != "" && vslid != null)
                {
                    ucVessel.SelectedValue = vslid;

                }

                SetBudgetCodeDetails();
                BindVesselAccount();
                BindData();
                BindOwnerBudgetCode();
                if (newreqid != null)
                {
                    BindEditWorkingGearItem();
                }
                else
                {
                    EditWorkingGearItem();
                    EditWorkGear(General.GetNullableGuid(Neededid));
                }
                gvRegistersworkinggearitem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                SetEmployeePrimaryDetails();
            }

            BindData();
            MainMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void Rebind()
    {
        gvRegistersworkinggearitem.SelectedIndexes.Clear();
        gvRegistersworkinggearitem.EditIndexes.Clear();
        gvRegistersworkinggearitem.DataSource = null;
        gvRegistersworkinggearitem.Rebind();
    }
    private void EditWorkGear(Guid? needid)
    {

        DataTable dt = PhoenixCrewWorkingGearItemIssue.WGNeededItemEdit(needid);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ViewState["confirm"] = dr["FLDCONFIRMEDYN"].ToString();

        }
        BindData();
    }

    public void EditWorkingGearItem()
    {
        DataTable dt = PhoenixCrewWorkingGearNeededItem.workinggearrequestedit(General.GetNullableGuid(Neededid));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ucVessel.SelectedValue = dr["FLDVESSELID"].ToString();
            txtBudgetCode.Text = dr["FLDBUDGETID"].ToString();
            BindVesselAccount();
            if (dr["FLDVESSELACCOUNTID"].ToString() != null && dr["FLDVESSELACCOUNTID"].ToString() != "")
            {
                ddlAccountDetails.SelectedValue = dr["FLDVESSELACCOUNTID"].ToString();
            }
            else
            {
                ddlAccountDetails.SelectedValue = "";
            }
            ddlBudgetCode.SelectedValue = dr["FLDBUDGETID"].ToString();
            OnBudgetChange(null, null);
            ddlOwnerBudgetCode.SelectedValue = dr["FLDOWNERBUDGETCODE"].ToString();
            ucZone.SelectedZone = dr["FLDZONEID"].ToString();
            ddlPayby.SelectedValue = dr["FLDPAYABLEBY"].ToString();
            txtRefNo.Text = dr["FLDREFNUMBER"].ToString();
            txtRequestDate.Text = dr["FLDREQUESTDATE"].ToString();
            txtid.Text = dr["FLDCREWWGNEEDEDITEMID"].ToString();
        }
    }

    public void BindEditWorkingGearItem()
    {
        DataTable dt = PhoenixCrewWorkingGearNeededItem.workinggearrequestedit(General.GetNullableGuid(Neededid));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ucVessel.SelectedValue = dr["FLDVESSELID"].ToString();
            txtBudgetCode.Text = dr["FLDBUDGETID"].ToString();
            BindVesselAccount();
            OnBudgetChange(null, null);
        }
    }


    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(empid));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Menuitems_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("REQUESTALL"))
            {

                if (!IsValidRequestLineItemRequest("1", "1"))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewWorkingGearRequest.workinggearitemrequest(General.GetNullableGuid(crewplanid)
                                                                    , General.GetNullableGuid(Neededid)
                                                                    , 2
                                                                    , null);
                BindData();
                gvRegistersworkinggearitem.Rebind();
                ucStatus.Text = "Working Item request created Successfully.";
            }
            if (CommandName.ToUpper().Equals("ISSUEALL"))
            {
                if (!IsValidRequestLineItemIssue("1"))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewWorkingGearItemIssue.workinggearitemIssueall(General.GetNullableGuid(crewplanid), null, General.GetNullableGuid(Neededid));
                BindData();
                gvRegistersworkinggearitem.Rebind();
                ucStatus.Text = "Working Item Issued Successfully.";
            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                if (crewplanid != null)
                {
                    // toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewWorkingGearOrderConfirm.aspx?back=yes&vesslid=" + vslid + "&empid=" + empid + "&rankid=" + rnkid + "&crewplanid=" + crewplanid + "&Neededid=" + Neededid + "&r=" + r + "',false,800,400); return false;", "Confirm", "<i class=\"fas fa-award\"></i>", "CONFIRM");
                    String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewWorkingGearOrderConfirm.aspx?back=yes&vesslid=" + vslid + "&empid=" + empid + "&rankid=" + rnkid + "&crewplanid=" + crewplanid + "&Neededid=" + Neededid + "&r=" + r + "');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                else
                {
                    ucError.ErrorMessage = "you cannot confirm here";
                    ucError.Visible = true;
                }
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void worknggearmain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../Crew/CrewWorkingGearOrderForm.aspx", false);

        }
        if (CommandName.ToUpper().Equals("REQUEST"))
        {
            Response.Redirect("../Crew/CrewWorkGearNeededItem.aspx?Neededid=" + Neededid + "&empid=" + empid + "&vesselid=" + vslid + "&crewplanid=" + crewplanid + "&r=" + r, false);
        }

        if (Neededid == null)
        {
            ucError.ErrorMessage = "Please create a request before proceed further.";
            ucError.Visible = true;
            return;
        }

        if (CommandName.ToUpper().Equals("RECEIVE"))
        {
            Response.Redirect("../Crew/CrewWorkingGearOrderReceive.aspx?Neededid=" + Neededid + "&empid=" + empid + "&vesselid=" + vslid + "&crewplanid=" + crewplanid + "&r=" + r, false);
        }
        if (CommandName.ToUpper().Equals("QUOTATION"))
        {
            Response.Redirect("../Crew/CrewWGQuotationAgentDetail.aspx?Neededid=" + Neededid + "&empid=" + empid + "&vesselid=" + vslid + "&crewplanid=" + crewplanid + "&r=" + r, false);
        }

    }
    protected void CrewWorkGearNeededItemRequest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidWorkGearRequest(empid, crewplanid, General.GetNullableString(ddlPayby.SelectedValue), General.GetNullableString(ucZone.SelectedZone), null))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid? resultneededid = null;

               PhoenixCrewWorkingGearNeededItem.NeededItemsSave(
               General.GetNullableInteger(empid)
               , General.GetNullableInteger(ucVessel.SelectedValue)
               , General.GetNullableGuid(crewplanid)
               , General.GetNullableInteger(ddlPayby.SelectedValue)
               , General.GetNullableInteger(ucZone.SelectedZone)
               //, General.GetNullableGuid(Neededid)
               , General.GetNullableDateTime(txtRequestDate.Text)
               , null
               , General.GetNullableInteger(ddlBudgetCode.SelectedValue)
               , General.GetNullableInteger(ddlAccountDetails.SelectedValue)
               , General.GetNullableGuid(ddlOwnerBudgetCode.SelectedValue)
                , ref resultneededid
               );


                ucStatus.Visible = true;
                ucStatus.Text = "Saved Successfully.";

                ViewState["resultneededid"] = Guid.Parse(resultneededid.ToString());
                txtid.Text = ViewState["resultneededid"].ToString();
                Neededid = ViewState["resultneededid"].ToString();

                EditWorkingGearItem();
                BindData();
                gvRegistersworkinggearitem.Rebind();
            }
            else if (CommandName.ToUpper().Equals("QUOTATION"))
            {
                Response.Redirect("../Crew/CrewWGQuotationAgentDetail.aspx?&neededid=" + Neededid);
            }
            else if (CommandName.ToUpper().Equals("REVERSE"))
            {

                PhoenixCrewWorkingGearRequest.workinggearitemcancel(General.GetNullableGuid(crewplanid), General.GetNullableGuid(Neededid), General.GetNullableGuid(orderid), null);

                txtRefNo.Text = "";
                txtRequestDate.Text = "";
                ucZone.SelectedZone = "";
                ddlPayby.SelectedValue = "";
                ddlPayby.Text = "";
                ddlAccountDetails.SelectedValue = "";
                ddlAccountDetails.Text = "";
                ddlOwnerBudgetCode.SelectedValue = "";
                ddlOwnerBudgetCode.Text = "";
                SetEmployeePrimaryDetails();
                BindData();
                gvRegistersworkinggearitem.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MainMenu()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        if (r != "1")
        {
            toolbarmain.AddButton("List", "LIST");
        }
        toolbarmain.AddButton("Request", "REQUEST");
        toolbarmain.AddButton("Quotation", "QUOTATION");
        toolbarmain.AddButton("Receive", "RECEIVE");
        worknggearmain.AccessRights = this.ViewState;
        worknggearmain.MenuList = toolbarmain.Show();

        if (r != "1")
        {
            worknggearmain.SelectedMenuIndex = 1;
        }
        else
        {
            worknggearmain.SelectedMenuIndex = 0;
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel Request", "REVERSE", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        CrewWorkGearNeededItemRequest.AccessRights = this.ViewState;
        CrewWorkGearNeededItemRequest.MenuList = toolbar.Show();
    }

    private bool IsValidRequestLineItem(string quantity, string orderquantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Requested Quantity is required.";
        }
        else if (General.GetNullableDecimal(quantity).Value <= 0)
        {
            ucError.ErrorMessage = "Requested  Quantity should be greater than zero.";
        }
        if (!General.GetNullableDecimal(orderquantity).HasValue)
        {
            ucError.ErrorMessage = "Order Quantity is required.";
        }
        else if (General.GetNullableDecimal(orderquantity).Value <= 0)
        {
            ucError.ErrorMessage = "Order Quantity should be greater than zero.";
        }

        return (!ucError.IsError);
    }
    private bool IsValidRequestLineItemRequest(string quantity, string UnitPrice)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Quantity is required.";
        }
        else if (General.GetNullableDecimal(quantity).Value <= 0)
        {
            ucError.ErrorMessage = "Quantity should be greater than zero.";
        }
        if (!General.GetNullableDecimal(UnitPrice).HasValue || UnitPrice == null)
        {
            ucError.ErrorMessage = "unit Price is required.";
        }
        else if (General.GetNullableDecimal(UnitPrice).Value <= 0)
        {
            ucError.ErrorMessage = "Unit Price should be greater than zero.";
        }
        if (ddlPayby.SelectedValue == "" || ddlPayby.SelectedValue == "dummy" || ddlPayby.SelectedValue == null)
        {
            ucError.ErrorMessage = " Payable By is required";
        }
        if (txtRequestDate.Text == null || txtRequestDate.Text.Trim() == "")
        {
            ucError.ErrorMessage = "Request date  is required";
        }

        return (!ucError.IsError);
    }
    private bool IsValidRequestLineItemIssue(string quantity)
    {
        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Quantity is required.";
        }
        else if (General.GetNullableDecimal(quantity).Value <= 0)
        {
            ucError.ErrorMessage = "Quantity should be greater than zero.";
        }
        if (ucZone.SelectedZone == "" || ucZone.SelectedZone == "dummy" || ucZone.SelectedZone == null)
        {
            ucError.ErrorMessage = "Zone is required";
        }
        if (ddlPayby.SelectedValue == "" || ddlPayby.SelectedValue == "dummy" || ddlPayby.SelectedValue == null)
        {
            ucError.ErrorMessage = " Payable By is required";
        }
        if (txtRequestDate.Text == null || txtRequestDate.Text.Trim() == "")
        {
            ucError.ErrorMessage = "Request date  is required";
        }

        return (!ucError.IsError);
    }
    private bool IsValidWorkGearRequest(string empid, string crewplanid, string ddlPayby, string zoneid, string neededid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(ddlPayby) || ddlPayby == "-Select-" || ddlPayby == null)
            ucError.ErrorMessage = "Payable By is required";
        if (zoneid == "Dummy" || zoneid == null)
            ucError.ErrorMessage = "Zone is required";
        if (txtRequestDate.Text == null || txtRequestDate.Text == "")
            ucError.ErrorMessage = "Request date  is required";

        return (!ucError.IsError);
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDQUANTITY", "FLDORDERQUANTITY", "FLDISSUED", "FLDRECEIVEDQUANTITY" };
            string[] alCaptions = { "Working Gear Item", "Requested Quantity", "Order Quantity", "Issued Quantity", "Received Quantity" };

            string sortexpression = (ViewState["MSORTEXPRESSION"] == null) ? null : (ViewState["MSORTEXPRESSION"].ToString());
            int? sortdirection = 1; //default desc order
            if (ViewState["MSORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["MSORTDIRECTION"].ToString());
            DataSet ds = new DataSet();
            if ((ViewState["NEEDEDREQID"].ToString() == "") && (newreqid == "1"))
            {
                ds = PhoenixCrewWorkingGearNeededItem.neededitemsearch(General.GetNullableInteger(vslid)
                                                                   , General.GetNullableInteger(null)
                                                                   , General.GetNullableGuid(null)
                                                                   , General.GetNullableGuid(null)
                                                                   , General.GetNullableGuid(null)
                                                                   , General.GetNullableInteger(r)
                                                                   , (int)ViewState["MPAGENUMBER"]
                                                                   , gvRegistersworkinggearitem.PageSize
                                                                   , ref iRowCount
                                                                   , ref iTotalPageCount);


            }
            else
            {

                ds = PhoenixCrewWorkingGearNeededItem.neededitemsearch(General.GetNullableInteger(ucVessel.SelectedValue)
                                                                            , General.GetNullableInteger(empid)
                                                                            , General.GetNullableGuid(crewplanid)
                                                                            , General.GetNullableGuid(Neededid)
                                                                            , General.GetNullableGuid(orderid)
                                                                            , General.GetNullableInteger(r)
                                                                            , (int)ViewState["MPAGENUMBER"]
                                                                            , gvRegistersworkinggearitem.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);

            }

            General.SetPrintOptions("gvRegistersworkinggearitem", "Working Gear Request", alCaptions, alColumns, ds);


            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../Crew/CrewWorkGearNeededItem.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvRegistersworkinggearitem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewWorkingGearItemSelection.aspx?back=yes&vesslid=" + vslid + "&empid=" + empid + "&rankid=" + rnkid + "&crewplanid=" + crewplanid + "&Neededid=" + Neededid + "&r=" + r + "&Orderback=" + Orderback + "'); return false;", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewWorkGearNeededItem.aspx", "Issue all", "<i class=\"fas fa-check-circle-ei\"></i>", "ISSUEALL");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewWorkGearNeededItem.aspx", "Confirm", "<i class=\"fas fa-award\"></i>", "CONFIRM");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('NAFA', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=WORKGEARISSUEDITEM&Neededid=" + Neededid + "&showmenu=1');return false;", "Show pdf", "<i class=\"fas fa-file-pdf\"></i>", "REPORT");

            Menuitems.AccessRights = this.ViewState;
            Menuitems.MenuList = toolbargrid.Show();
            SessionUtil.PageAccessRights(this.ViewState);

            gvRegistersworkinggearitem.DataSource = ds;
            gvRegistersworkinggearitem.VirtualItemCount = iRowCount;

            ViewState["MROWCOUNT"] = iRowCount;
            ViewState["MTOTALPAGECOUNT"] = iTotalPageCount;

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
            string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDQUANTITY", "FLDORDERQUANTITY", "FLDISSUED", "FLDRECEIVEDQUANTITY" };
            string[] alCaptions = { "Working Gear Item", "Requested Quantity", "Order Quantity", "Issued Quantity", "Received Quantity" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = new DataSet();
            if ((ViewState["NEEDEDREQID"].ToString() == "") && (newreqid == "1"))
            {
                ds = PhoenixCrewWorkingGearNeededItem.neededitemsearch(General.GetNullableInteger(vslid)
                                                                   , General.GetNullableInteger(null)
                                                                   , General.GetNullableGuid(null)
                                                                   , General.GetNullableGuid(null)
                                                                   , General.GetNullableGuid(null)
                                                                   , General.GetNullableInteger(r)
                                                                   , (int)ViewState["MPAGENUMBER"]
                                                                   , gvRegistersworkinggearitem.PageSize
                                                                   , ref iRowCount
                                                                   , ref iTotalPageCount);


            }
            else
            {

                ds = PhoenixCrewWorkingGearNeededItem.neededitemsearch(General.GetNullableInteger(ucVessel.SelectedValue)
                                                                            , General.GetNullableInteger(empid)
                                                                            , General.GetNullableGuid(crewplanid)
                                                                            , General.GetNullableGuid(Neededid)
                                                                            , General.GetNullableGuid(orderid)
                                                                            , General.GetNullableInteger(r)
                                                                            , (int)ViewState["MPAGENUMBER"]
                                                                            , gvRegistersworkinggearitem.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);

            }

            General.ShowExcel("Working Gear Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRegistersworkinggearitem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["MPAGENUMBER"] = ViewState["MPAGENUMBER"] != null ? ViewState["MPAGENUMBER"] : gvRegistersworkinggearitem.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvRegistersworkinggearitem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) if (!SessionUtil.CanAccess(this.ViewState, edit.CommandName)) edit.Visible = false;

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, del.CommandName)) del.Visible = false;
                del.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete the item?')");
            }

            LinkButton Issue = (LinkButton)e.Item.FindControl("imgIssue");
            if (Issue != null) Issue.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to issue the item? ?')");

            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (drv["FLDISSUEORREQUESTYN"] != null && drv["FLDISSUEORREQUESTYN"].ToString() != "")
            {

                if (Issue != null) Issue.Visible = false;

                LinkButton request = (LinkButton)e.Item.FindControl("imgRequest");
                if (request != null) request.Visible = false;

                if (del != null) del.Visible = false;

                LinkButton Cancel = (LinkButton)e.Item.FindControl("ImageCancel");
                if (Cancel != null) Cancel.Visible = true;
                if (Cancel != null)
                {
                    Cancel.Visible = SessionUtil.CanAccess(this.ViewState, Cancel.CommandName);
                    Cancel.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Cancel Issue ?')");
                }

                RadLabel lblunitPriceedit = (RadLabel)e.Item.FindControl("lblunitPriceedit");
                if (lblunitPriceedit != null) lblunitPriceedit.Visible = true;

                RadLabel lblGearitemQuantityedite = (RadLabel)e.Item.FindControl("lblGearitemQuantityedite");
                if (lblGearitemQuantityedite != null) lblGearitemQuantityedite.Visible = true;

                RadLabel lblOrderQuantityedit = (RadLabel)e.Item.FindControl("lblOrderQuantityedit");
                if (lblOrderQuantityedit != null) lblOrderQuantityedit.Visible = true;

                UserControlMaskNumber txtQuantityEdit = (UserControlMaskNumber)e.Item.FindControl("txtQuantityEdit");
                if (txtQuantityEdit != null) txtQuantityEdit.Visible = false;

                UserControlMaskNumber txtOrderQuantityEdit = (UserControlMaskNumber)e.Item.FindControl("txtOrderQuantityEdit");
                if (txtOrderQuantityEdit != null) txtOrderQuantityEdit.Visible = false;

                if (edit != null) edit.Visible = false;

            }
        }
    }

    protected void gvRegistersworkinggearitem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("REQUEST"))
            {
                string itemid = ((RadLabel)e.Item.FindControl("lblitemid")).Text;
                String quantity = ((RadLabel)e.Item.FindControl("lblGearitemQuantityitem")).Text;
                String lineitemid = ((RadLabel)e.Item.FindControl("lblitemid")).Text;
                String UnitPrice = ((RadLabel)e.Item.FindControl("lblunitPrice")).Text;

                if (!IsValidRequestLineItemRequest(quantity, UnitPrice))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewWorkingGearRequest.workinggearitemrequest(General.GetNullableGuid(crewplanid)
                                                                    , General.GetNullableGuid(Neededid)
                                                                    , 2
                                                                    , General.GetNullableGuid(itemid));

                BindData();
                gvRegistersworkinggearitem.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("ISSUE"))
            {
                string itemid = ((RadLabel)e.Item.FindControl("lblitemid")).Text;
                String quantity = ((RadLabel)e.Item.FindControl("lblGearitemQuantityitem")).Text;
                String lineitemid = ((RadLabel)e.Item.FindControl("lblitemid")).Text;

                if (!IsValidRequestLineItemIssue(quantity))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewWorkingGearItemIssue.workinggearitemIssueall(General.GetNullableGuid(crewplanid), General.GetNullableGuid(itemid), General.GetNullableGuid(Neededid));

                BindData();
                gvRegistersworkinggearitem.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("CANCELREQ"))
            {
                string itemid = ((RadLabel)e.Item.FindControl("lblitemid")).Text;
                PhoenixCrewWorkingGearRequest.workinggearitemcancel(General.GetNullableGuid(crewplanid), General.GetNullableGuid(Neededid), General.GetNullableGuid(orderid), General.GetNullableGuid(itemid));

                BindData();
                gvRegistersworkinggearitem.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string Requestquantity = ((UserControlMaskNumber)e.Item.FindControl("txtQuantityEdit")).Text;
                string orderquntity = ((UserControlMaskNumber)e.Item.FindControl("txtOrderQuantityEdit")).Text;
                if (!IsValidRequestLineItem(Requestquantity, orderquntity))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

            }
            else if (e.CommandName == "Page")
            {
                ViewState["MPAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvRegistersworkinggearitem_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        string lineItemId = ((RadLabel)e.Item.FindControl("lbllineitemid")).Text;
        PhoenixCrewWorkingGearNeededItem.deleteNeededItems(new Guid(lineItemId));

        BindData();
        gvRegistersworkinggearitem.Rebind();
    }

    protected void gvRegistersworkinggearitem_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void VesselChanged(object sender, EventArgs e)
    {
        if ((General.GetNullableInteger(ucVessel.SelectedValue) != null || vslid != ""))
        {
            string strvesselid = null;
            if (General.GetNullableInteger(ucVessel.SelectedValue) == null)
            {
                strvesselid = vslid;
            }
            else
            {
                strvesselid = ucVessel.SelectedValue;
            }
            BindVesselAccount();

            if (ddlBudgetCode.SelectedValue != "")
            {
                ddlOwnerBudgetCode.SelectedValue = "";
                ddlOwnerBudgetCode.Text = "";

                DataSet ds1 = PhoenixCrewWorkingGearRequest.ListOwnerWorkingGearBudgetCode(
                            General.GetNullableInteger(ucVessel.SelectedValue)
                            , Convert.ToInt32(ddlBudgetCode.SelectedValue)
                            , General.GetNullableInteger(ddlAccountDetails.SelectedValue));
                ddlOwnerBudgetCode.DataSource = ds1;
                ddlOwnerBudgetCode.DataTextField = "FLDOWNERBUDGETCODE";
                ddlOwnerBudgetCode.DataValueField = "FLDOWNERBUDGETCODEMAPID";
                ddlOwnerBudgetCode.DataBind();
                ddlOwnerBudgetCode.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                if (ds1.Tables[0].Rows.Count == 1)
                {
                    ddlOwnerBudgetCode.SelectedValue = ds1.Tables[0].Rows[0]["FLDOWNERBUDGETCODEMAPID"].ToString();
                }
            }

        }
    }
    private void SetBudgetCodeDetails()
    {
        DataSet ds = PhoenixRegistersBudget.ListBudget(102);
        ddlBudgetCode.DataSource = ds;
        ddlBudgetCode.DataTextField = "FLDDESCRIPTION";
        ddlBudgetCode.DataValueField = "FLDBUDGETID";
        ddlBudgetCode.DataBind();
        ddlBudgetCode.SelectedValue = "17";
        OnBudgetChange(null, null);
    }

    protected void BindOwnerBudgetCode()
    {

        ddlOwnerBudgetCode.SelectedValue = "";
        ddlOwnerBudgetCode.Text = "";

        DataSet ds1 = PhoenixCrewWorkingGearRequest.ListOwnerWorkingGearBudgetCode(General.GetNullableInteger(ucVessel.SelectedValue), Convert.ToInt32(ddlBudgetCode.SelectedValue), General.GetNullableInteger(ddlAccountDetails.SelectedValue));
        if (ds1.Tables[0].Rows.Count > 0)
        {
            ViewState["OWNBUDCOUNT"] = ds1.Tables[0].Rows.Count;
            ddlOwnerBudgetCode.DataSource = ds1;
            ddlOwnerBudgetCode.DataTextField = "FLDOWNERBUDGETCODE";
            ddlOwnerBudgetCode.DataValueField = "FLDOWNERBUDGETCODEMAPID";
            ddlOwnerBudgetCode.DataBind();
            ddlOwnerBudgetCode.Items.Insert(0, new RadComboBoxItem("--Select--", ""));


        }
        else
        {
            ViewState["OWNBUDCOUNT"] = null;
            ddlOwnerBudgetCode.Items.Clear();
            ddlOwnerBudgetCode.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        }
    }
    protected void OnBudgetChange(object sender, EventArgs e)
    {
        if (ddlBudgetCode.SelectedValue != "")
        {
            DataSet ds = PhoenixRegistersBudget.EditBudget(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(ddlBudgetCode.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtBudgetCode.Text = ds.Tables[0].Rows[0]["FLDSUBACCOUNT"].ToString();
            }
            BindOwnerBudgetCode();
        }
        else
        {
            txtBudgetCode.Text = "";
            ddlOwnerBudgetCode.Items.Clear();
            ddlOwnerBudgetCode.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

        }
    }
    public void BindVesselAccount()
    {
        ddlAccountDetails.SelectedValue = "";
        ddlAccountDetails.Text = "";

        DataSet dsl = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
            General.GetNullableInteger(ucVessel.SelectedValue) == 0 ? null : General.GetNullableInteger(ucVessel.SelectedValue), 1);
        ddlAccountDetails.DataSource = dsl;
        ddlAccountDetails.DataBind();

        if (dsl.Tables[0].Rows.Count == 1)
        {
            ddlAccountDetails.SelectedValue = "";
            ddlAccountDetails.Text = "";
            ddlAccountDetails.SelectedValue = dsl.Tables[0].Rows[0]["FLDACCOUNTID"].ToString();
        }

    }

    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void ddlAccountDetails_TextChanged(object sender, EventArgs e)
    {
        BindOwnerBudgetCode();
    }
    protected void gvRegistersworkinggearitem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        string quantitye = ((UserControlMaskNumber)e.Item.FindControl("txtQuantityEdit")).Text;
        string lineitemide = ((RadLabel)e.Item.FindControl("lbllineitemidedit")).Text;
        string receivedquantity = ((RadLabel)e.Item.FindControl("txtRQuantityEdit")).Text;
        string txtOrderQuantityEdit = ((UserControlMaskNumber)e.Item.FindControl("txtOrderQuantityEdit")).Text;

        if (!IsValidRequestLineItem(quantitye, txtOrderQuantityEdit))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixCrewWorkingGearNeededItem.SaveNeededLineItems(General.GetNullableInteger(quantitye)
                                                            , new Guid(lineitemide)
                                                            , General.GetNullableDecimal(null)
                                                            , General.GetNullableInteger(receivedquantity)
                                                            , General.GetNullableInteger(txtOrderQuantityEdit)
                                                            , null);

        BindData();
        gvRegistersworkinggearitem.Rebind();
    }
}
