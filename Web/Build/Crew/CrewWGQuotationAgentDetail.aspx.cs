using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.CrewManagement;
using System.Web;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewWGQuotationAgentDetail : PhoenixBasePage
{
    private string Neededid = string.Empty;
    private string Agentid = string.Empty;

    private string empid = string.Empty;
    private string vslid = string.Empty;
    private string crewplanid = null;
    private string orderid = null;
    private string r = null;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"] != "")
                ViewState["planid"] = Request.QueryString["crewplanid"];
            if (Request.QueryString["vslid"] != null && Request.QueryString["vslid"] != "")
                ViewState["vslid"] = Request.QueryString["vslid"];
            if (Request.QueryString["empid"] != null && Request.QueryString["empid"] != "")
                ViewState["empid"] = Request.QueryString["empid"];
            if (Request.QueryString["Neededid"] != null && Request.QueryString["Neededid"] != "")
                ViewState["NEEDEDID"] = Request.QueryString["Neededid"];
            if (Request.QueryString["orderid"] != null && Request.QueryString["orderid"] != "")
                ViewState["ORDERID"] = Request.QueryString["orderid"];
            if (Request.QueryString["r"] != null && Request.QueryString["r"] != "")
                ViewState["r"] = Request.QueryString["r"];
          

            if (ViewState["empid"] != null)
                empid = ViewState["empid"].ToString();
            if (ViewState["vslid"] != null)
                vslid = ViewState["vslid"].ToString();
            if (ViewState["planid"] != null)
                crewplanid = ViewState["planid"].ToString();
            if (ViewState["NEEDEDID"] != null)
                Neededid = ViewState["NEEDEDID"].ToString();
            if (ViewState["ORDERID"] != null)
                orderid = ViewState["ORDERID"].ToString();
            if (ViewState["r"] != null)
                r = ViewState["r"].ToString();
          
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            if (r != "1")
            {
                toolbarmain.AddButton("List", "LIST");
            }

            toolbarmain.AddButton("Request", "REQUEST");
            toolbarmain.AddButton("Quotation", "QUOTATION");
            toolbarmain.AddButton("Receive", "RECEIVE");
            MenuAgent.AccessRights = this.ViewState;
            MenuAgent.MenuList = toolbarmain.Show();
            if (r != "1")
            {
                MenuAgent.SelectedMenuIndex = 2;
            }
            else
            {
                MenuAgent.SelectedMenuIndex = 1;
            }


            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewWGQuotationAgentDetail.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAgent')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            // toolbar.AddFontAwesomeButton("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewListAddressAgent.aspx?Neededid=" + Neededid + "');return false;", "Add Agent", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("../Crew/CrewWGQuotationAgentDetail.aspx", "Add Agent", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("../Crew/CrewWGQuotationAgentDetail.aspx", "Send Query", "<i class=\"fas fa-envelope\"></i>", "RFQ");
            toolbar.AddFontAwesomeButton("../Crew/CrewWGQuotationAgentDetail.aspx", "Send Reminder", "<i class=\"fas fa-bell\"></i>", "RFQREMAINDER");
            MenuAgentList.AccessRights = this.ViewState;
            MenuAgentList.MenuList = toolbar.Show();
           
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["REQUISITIONNO"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["confirm"] = "";

                gvAgent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["LPAGENUMBER"] = 1;
                ViewState["LSORTEXPRESSION"] = null;
                ViewState["LSORTDIRECTION"] = null;
                gvRegistersworkinggearitem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                EditWorkGear(General.GetNullableGuid(Neededid));

            }

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewWGQuotationAgentDetail.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL1");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvRegistersworkinggearitem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT1");
            MenuRegistersWorkingGearItem.AccessRights = this.ViewState;
            MenuRegistersWorkingGearItem.MenuList = toolbar1.Show();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindTitle()
    {
        PhoenixToolbar toolbarTitle = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.Title = lblTitle.Text;
        MenuTitle.MenuList = toolbarTitle.Show();
    }

    protected void MenuAgent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("REQUEST"))
            {
                Response.Redirect("../Crew/CrewWorkGearNeededItem.aspx?Neededid=" + Neededid + "&empid=" + empid + "&vesselid=" + vslid + "&crewplanid=" + crewplanid + "&r=" + r, false);

            }
            if (CommandName.ToUpper().Equals("RECEIVE"))
            {
                Response.Redirect("../Crew/CrewWorkingGearOrderReceive.aspx?Neededid=" + Neededid + "&empid=" + empid + "&vesselid=" + vslid + "&crewplanid=" + crewplanid + "&r=" + r, false);
            }
            if (CommandName.ToUpper().Equals("QUOTATION"))
            {

                Response.Redirect("../Crew/CrewWGQuotationAgentDetail.aspx?Neededid=" + Neededid + "&empid=" + empid + "&vesselid=" + vslid + "&crewplanid=" + crewplanid + "&r=" + r, false);
            }
            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Crew/CrewWorkingGearOrderForm.aspx", false);

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    protected void MenuAgentList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("RFQ"))
            {
                SendForQuotation();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("RFQREMAINDER"))
            {
                SendReminderForQuotation();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
              
                if (ViewState["confirm"].ToString() == "" || ViewState["confirm"].ToString() == "0")
                {
                                    
                    String scriptpopup = String.Format("javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewListAddressAgent.aspx?Neededid=" + Neededid + "');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                else
                {
                    ucError.ErrorMessage = "already confirmed";
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


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDAGENTNAME", "FLDQUERYSENTDATE", "FLDQUOTEDRECEIVEDDATE", "FLDPOSENTDATE", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Supplier", "Query Sent", "Quotation Received", "Purchase Order Sent", " 	Total Amount" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewWorkingGearNeededItem.CrewWGAgentSearch(new Guid(Neededid), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvAgent.PageSize, ref iRowCount, ref iTotalPageCount);

        General.ShowExcel("Working Gear Suppliers", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDAGENTNAME", "FLDQUERYSENTDATE", "FLDQUOTEDRECEIVEDDATE", "FLDPOSENTDATE", "FLDTOTALAMOUNT" };
            string[] alCaptions = { "Supplier", "Query Sent", "Quotation Received", "Purchase Order Sent", " 	Total Amount" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewWorkingGearNeededItem.CrewWGAgentSearch(General.GetNullableGuid(Neededid)
                                                                            , sortexpression
                                                                            , sortdirection
                                                                            , (int)ViewState["PAGENUMBER"]
                                                                            , gvAgent.PageSize
                                                                            , ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvAgent", "Working Gear Suppliers", alCaptions, alColumns, ds);

            gvAgent.DataSource = ds;
            gvAgent.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAgent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAgent.CurrentPageIndex + 1;

        BindData();
    }


    protected void gvAgent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, edit.CommandName)) edit.Visible = false;
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton sm = (LinkButton)e.Item.FindControl("cmdSemdMail");
            LinkButton cmdApprove = (LinkButton)e.Item.FindControl("cmdApprove");

            RadLabel Needed = (RadLabel)e.Item.FindControl("lblNeededId");
            RadLabel Agent = (RadLabel)e.Item.FindControl("lblAgentID");
            if (sm != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, sm.CommandName)) sm.Visible = false;
                sm.Attributes.Add("onclick", "openNewWindow('NAFA', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=WORKINGGEARREQUEST&Neededid=" + Needed.Text + "&Agentid=" + Agent.Text + "&showmenu=0&showword=no&showexcel=no&emailyn=2');return false;");
            }

            if (cmdApprove != null)
            {
                cmdApprove.Visible = SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName);
                cmdApprove.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to approve?')");
            }

            RadLabel approveyn = (RadLabel)e.Item.FindControl("lblApproveYN");

            LinkButton SendPomail = (LinkButton)e.Item.FindControl("cmdSemdMail");

            if (approveyn != null)
            {
                if (approveyn.Text == "1")
                { SendPomail.Visible = true; }

                else

                { SendPomail.Visible = false; }
            }

        }

        if (e.Item.IsInEditMode)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            UserControlCurrency ucCurrencyEdit = (UserControlCurrency)e.Item.FindControl("ucCurrencyEdit");
            if (ucCurrencyEdit != null) ucCurrencyEdit.SelectedCurrency = drv["FLDCURRENCYID"].ToString();

            UserControlCurrency ucCurrency = (UserControlCurrency)e.Item.FindControl("ucCurrencyEdit");
            if (ucCurrency != null)
            {
                ucCurrency.CurrencyList = PhoenixRegistersCurrency.ListActiveCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , true);
                ucCurrency.DataBind();

                DataRowView dr = (DataRowView)e.Item.DataItem;
                if (dr != null)
                    ucCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            }

        }

    }

    protected void gvAgent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("APPROVEANDFINALIZE"))
            {
                PhoenixCrewWorkingGearQuotation.approvequote(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(Neededid)
                                                            , int.Parse(((RadLabel)e.Item.FindControl("lblAGentID")).Text));

                BindData();
                gvAgent.Rebind();
            }

            if (e.CommandName.ToUpper() == "SELECT")
            {
                ViewState["NEEDEDID"] = ((RadLabel)e.Item.FindControl("lblNeededId")).Text;
                ViewState["AGENTID"] = ((RadLabel)e.Item.FindControl("lblAgentID")).Text;
                Agentid = ((RadLabel)e.Item.FindControl("lblAgentID")).Text;
                lbllineitem.Text = "Items - " + ((LinkButton)e.Item.FindControl("lnkAgentName")).Text;
                BindData();
                gvAgent.Rebind();

                BindDataLineItem();
                gvRegistersworkinggearitem.Rebind();
            }
            if (e.CommandName.ToUpper() == "SENDMAIL")
            {

            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvAgent_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string currencyid = ((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit")).SelectedCurrency;

            PhoenixCrewWorkingGearQuotation.UpdateQuotationAgent(new Guid(Neededid),
                                            int.Parse(((RadLabel)e.Item.FindControl("lblAgentIDEdit")).Text)
                                            , General.GetNullableInteger(currencyid)
                );

            BindData();
            gvAgent.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvAgent_DeleteCommand(object sender, GridCommandEventArgs e)

    {
        try
        {
            PhoenixCrewWorkingGearQuotation.DeleteSupplier(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                           , new Guid(Neededid)
                                                           , int.Parse(((RadLabel)e.Item.FindControl("lblAGentID")).Text));

            BindData();
            gvAgent.Rebind();

            lbllineitem.Text = "Items";

            BindDataLineItem();
            gvRegistersworkinggearitem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAgent_SortCommand(object sender, GridSortCommandEventArgs e)
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


    private void SendForQuotation()
    {
        try
        {
            string selectedvendors = ",";
            foreach (GridDataItem gvr in gvAgent.Items)
            {
                if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)(gvr.FindControl("chkSelect"))).Checked == true)
                {
                    selectedvendors = selectedvendors + ((RadLabel)(gvr.FindControl("lblAgentID"))).Text + ",";
                }
            }

            if (selectedvendors.Length <= 1)
                selectedvendors = null;

            DataSet dsvendor = PhoenixCrewWorkingGearQuotation.ListCrewWorkingGearQtToSendValidate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(Neededid), selectedvendors);

            if (dsvendor.Tables[0].Rows.Count > 0)
            {
                string scriptRefreshDontClose = "";
                scriptRefreshDontClose += "<script language='javaScript' id='CrewWGEmail'>" + "\n";
                scriptRefreshDontClose += "parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewWorkingGearEmail.aspx?purpose=RFQ&Neededid=" + Neededid + "&selectedagent=" + selectedvendors + "')";
                scriptRefreshDontClose += "</script>" + "\n";
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "CrewWGEmail", scriptRefreshDontClose, false);

            }
            else
            {
                ucError.ErrorMessage = "Email already sent";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SendReminderForQuotation()
    {
        try
        {
            string selectedvendors = ",";
            foreach (GridDataItem gvr in gvAgent.Items)
            {
                if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)(gvr.FindControl("chkSelect"))).Checked == true)
                {
                    selectedvendors = selectedvendors + ((RadLabel)(gvr.FindControl("lblAgentID"))).Text + ",";
                }
            }

            if (selectedvendors.Length <= 1)
                selectedvendors = null;
            DataSet dsvendor = PhoenixCrewWorkingGearQuotation.ListCrewWorkingGearQtToSendRemainder(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(Neededid), selectedvendors);

            if (dsvendor.Tables[0].Rows.Count > 0)
            {
                string scriptRefreshDontClose = "";
                scriptRefreshDontClose += "<script language='javaScript' id='CrewWGEmail'>" + "\n";
                scriptRefreshDontClose += "parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewWorkingGearEmail.aspx?purpose=RFQREMAINDER&Neededid=" + Neededid + "&selectedagent=" + selectedvendors + "');";
                scriptRefreshDontClose += "</script>" + "\n";
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "CrewWGEmail", scriptRefreshDontClose, false);

            }
            else
            {
                ucError.ErrorMessage = "No agent selected to send reminder";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDate(string receiveddate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (receiveddate == null || receiveddate == "")
        {
            ucError.ErrorMessage = "Quotation Received Date is required.";
        }

        return (!ucError.IsError);
    }

    private bool IsValidAgent(string agentid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        return (!ucError.IsError);
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvAgent.Rebind();

            if (Session["emailsend"] != null)
            {
                ucConfirm.ErrorMessage = "E-mail sent to ,<BR/>" + Session["emailsend"].ToString();
                ucConfirm.Visible = true;
            }
            Session["emailsend"] = null;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersWorkingGearItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL1"))
        {
            ShowExcel1();
        }
    }
    protected void ShowExcel1()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDREQUESTEDQUANTITY", "FLDAMOUNT", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Working Gear Item", "Quantity", "Unit Price", "Total Price " };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["LSORTEXPRESSION"] == null) ? null : (ViewState["LSORTEXPRESSION"].ToString());
        if (ViewState["LSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["LSORTDIRECTION"].ToString());

        if (ViewState["LROWCOUNT"] == null || Int32.Parse(ViewState["LROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["LROWCOUNT"].ToString());

        ds = PhoenixCrewWorkingGearQuotation.CrewWGAgentitemSearch(General.GetNullableGuid(Neededid)
                                                                         , General.GetNullableInteger(ViewState["AGENTID"] != null ? ViewState["AGENTID"].ToString() : Agentid)
                                                                         , sortexpression
                                                                         , sortdirection
                                                                         , Int32.Parse(ViewState["LPAGENUMBER"].ToString())
                                                                         , gvRegistersworkinggearitem.PageSize
                                                                         , ref iRowCount
                                                                         , ref iTotalPageCount
                                                             );


        General.ShowExcel("Working Gear Items", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }


    private void BindDataLineItem()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDREQUESTEDQUANTITY", "FLDAMOUNT", "FLDTOTALAMOUNT" };
            string[] alCaptions = { "Working Gear Item", "Quantity", "Unit Price", "Total Price " };

            string sortexpression = (ViewState["LSORTEXPRESSION"] == null) ? null : (ViewState["LSORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["LSORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["LSORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewWorkingGearQuotation.CrewWGAgentitemSearch(General.GetNullableGuid(Neededid)
                                                                                , General.GetNullableInteger(ViewState["AGENTID"] != null ? ViewState["AGENTID"].ToString() : Agentid)
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , Int32.Parse(ViewState["LPAGENUMBER"].ToString())
                                                                                , gvRegistersworkinggearitem.PageSize
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount
                                                                    );

            General.SetPrintOptions("gvRegistersworkinggearitem", "Working Gear Item", alCaptions, alColumns, ds);

            gvRegistersworkinggearitem.DataSource = ds;
            gvRegistersworkinggearitem.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblTitle.Text = "Quotation" + " [" + ds.Tables[0].Rows[0]["FLDREFNUMBER"].ToString() + "] ";
                BindTitle();
            }


            ViewState["LROWCOUNT"] = iRowCount;
            ViewState["LTOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRegistersworkinggearitem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["LPAGENUMBER"] = ViewState["LPAGENUMBER"] != null ? ViewState["LPAGENUMBER"] : gvRegistersworkinggearitem.CurrentPageIndex + 1;

        BindDataLineItem();
    }

    protected void gvRegistersworkinggearitem_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, edit.CommandName)) edit.Visible = false;
            }

        }
    }

    protected void gvRegistersworkinggearitem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["LPAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    // protected void Rebind()
    // {
    //     gvAgent.SelectedIndexes.Clear();
    //     gvAgent.EditIndexes.Clear();
    //     gvAgent.DataSource = null;
    //     gvAgent.Rebind();
    // }


    protected void gvRegistersworkinggearitem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (ViewState["AGENTID"] != null)
            {

                PhoenixCrewWorkingGearQuotation.Updatelineitemprice(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(Neededid)
                , int.Parse(ViewState["AGENTID"].ToString())
                , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblworkinggearlineEdit")).Text)
                , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtRQuantityEdit")).Text)
                );

                BindDataLineItem();
                gvRegistersworkinggearitem.Rebind();

                BindData();
                gvAgent.Rebind();
            }
            else
            {
                ucError.ErrorMessage = "No agent selected to update price";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
}

