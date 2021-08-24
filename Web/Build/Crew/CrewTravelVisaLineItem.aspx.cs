using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
public partial class CrewTravelVisaLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            ViewState["TRAVELVISAID"] = null;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["RECORDSCOUNT"] = "0";
            ViewState["AGENTID"] = null;

            if (Request.QueryString["travelvisaid"] != null)
            {
                ViewState["TRAVELVISAID"] = Request.QueryString["travelvisaid"].ToString();
                ViewState["ACTIVE"] = "1";
            }
            if (ViewState["TRAVELVISAID"] != null)
                ListTravelVisaRequest(new Guid(ViewState["TRAVELVISAID"].ToString()));

            gvVisaLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

        if (ViewState["TRAVELVISAID"] != null)
        {
            gvVisaLineItem.Visible = true;
            lblSeafaresDetails.Visible = true;
            LineItemMenu();
            ucVessel.Enabled = false;
        }
        else
        {
            gvVisaLineItem.Visible = false;
            lblSeafaresDetails.Visible = false;
        }

        MainMenu();
        SaveMenu();
    }
    private void SaveMenu()
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuTravelVisaSub.AccessRights = this.ViewState;
        MenuTravelVisaSub.MenuList = toolbarsub.Show();

        if (ViewState["TRAVELVISAID"] == null)
            MenuTravelVisaSub.Visible = true;
        else
            MenuTravelVisaSub.Visible = false;
    }
    private void MainMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Visa Request", "VISAREQUEST");
        toolbar.AddButton("Request Details", "VISALINEITEM");
        MenuVisa.AccessRights = this.ViewState;
        MenuVisa.MenuList = toolbar.Show();

        MenuVisa.SelectedMenuIndex = 1;
    }

    private void LineItemMenu()
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();        
        toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelVisaLineItem.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvVisaLineItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelVisaLineItemAdd.aspx?travelvisaid=" + ViewState["TRAVELVISAID"].ToString() + "'); return false;", "Add Seafarers", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelVisaLineItem.aspx", "Email", "<i class=\"fas fa-envelope\"></i>", "Email");
        MenuLineItem.AccessRights = this.ViewState;
        MenuLineItem.MenuList = toolbargrid.Show();
    }


    protected void MenuVisa_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("VISAREQUEST"))
            {
                Response.Redirect("CrewTravelVisaRequest.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTravelVisaSub_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? travelvisaid = General.GetNullableGuid(ViewState["TRAVELVISAID"] != null ? ViewState["TRAVELVISAID"].ToString().ToString() : null);

            if (CommandName.ToUpper() == "SAVE")
            {
                if (ViewState["TRAVELVISAID"] == null)
                {
                    if (!IsValidTravelVisaRequest())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    else
                    {
                        PhoenixCrewTravelVisa.InsertCrewTravelVisa(
                                                 General.GetNullableInteger(ucVessel.SelectedVessel)
                                               , General.GetNullableDateTime(txtVisaDate.Text)
                                               , General.GetNullableInteger(ucMultiAgent.SelectedValue)
                                               , General.GetNullableInteger(ucCountry.SelectedCountry)
                                               , General.GetNullableString(txtRemarks.Text)
                                               , ref travelvisaid);
                        if (travelvisaid != null)
                        {
                            ViewState["TRAVELVISAID"] = travelvisaid.ToString();
                            gvVisaLineItem.Visible = true;
                            lblSeafaresDetails.Visible = true;
                            LineItemMenu();
                            ListTravelVisaRequest(new Guid(ViewState["TRAVELVISAID"].ToString()));
                        }
                    }
                }
                SaveMenu();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void ListTravelVisaRequest(Guid travelvisaid)
    {
        DataSet ds = PhoenixCrewTravelVisa.ListTravelVisaRequest(travelvisaid);
        DataTable dt = ds.Tables[0];
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtReqNo.Text = dt.Rows[0]["FLDREFERENCENO"].ToString();
            ucVessel.SelectedVessel = dt.Rows[0]["FLDVESSELID"].ToString();
            ucMultiAgent.SelectedValue = dt.Rows[0]["FLDAGENTID"].ToString();
            ucMultiAgent.Text = dt.Rows[0]["FLDAGENTNAME"].ToString();
            txtVisaDate.Text = dt.Rows[0]["FLDVISADATE"].ToString();
            ucCountry.SelectedCountry = dt.Rows[0]["FLDVISACOUNTRY"].ToString();
            txtRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
        }
    }

    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper() == "EMAIL")
        {
            if (ViewState["RECORDSCOUNT"].ToString() == "1")
            {
                string scriptRefreshDontClose = "";
                scriptRefreshDontClose += "<script language='javaScript' id='CrewTravelVisaEmail'>" + "\n";
                scriptRefreshDontClose += "parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewTravelVisaEmail.aspx?travelvisaid=" + ViewState["TRAVELVISAID"].ToString() + "')";
                scriptRefreshDontClose += "</script>" + "\n";
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "CrewEmail", scriptRefreshDontClose, false);
            }
            else
            {
                ucError.ErrorMessage = "Please add seafarers";
                ucError.Visible = true;
                return;
            }
        }
        if (CommandName.ToUpper() == "EXCEL")
        {
            ShowExcel();
        }

    }

    private void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDNAME", "FLDEMPLOYEECODE", "FLDRANKCODE", "FLDPASSPORTNO", "FLDVISAREQUESTVESSEL", "FLDJOINEDVESSELNAME", "FLDAMOUNT", "FLDPAYMENTMODENAME", "FLDVISAREASONNAME", "FLDAPPLIEDON", "FLDDOCRECEIVEDON", "FLDVISASTATUSNAME" };
            string[] alCaptions = { "Name", "File No.", "Rank Code", "Passport No.", "Visa Request Vessel", "Joined Vessel", "Visa Cost(USD)", "Payment By", "Reason", "Applied On", "Document Received on", "Visa Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;

            DataSet ds = new DataSet();

            ds = PhoenixCrewTravelVisa.CrewTravelVisaLineItemSearch(ViewState["TRAVELVISAID"] != null ? General.GetNullableGuid(ViewState["TRAVELVISAID"].ToString()) : null, sortexpression, sortdirection,
                                (int)ViewState["PAGENUMBER"],
                                gvVisaLineItem.PageSize,
                                ref iRowCount,
                                ref iTotalPageCount);

            if (ds.Tables.Count > 0)
                General.ShowExcel("Seafarer Details", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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

            string[] alColumns = { "FLDNAME", "FLDEMPLOYEECODE", "FLDRANKCODE", "FLDPASSPORTNO", "FLDVISAREQUESTVESSEL", "FLDJOINEDVESSELNAME", "FLDAMOUNT", "FLDPAYMENTMODENAME", "FLDVISAREASONNAME", "FLDAPPLIEDON", "FLDDOCRECEIVEDON", "FLDVISASTATUSNAME" };
            string[] alCaptions = { "Name", "File No.", "Rank Code", "Passport No.", "Visa Request Vessel", "Joined Vessel", "Visa Cost(USD)", "Payment By", "Reason", "Applied On", "Document Received on", "Visa Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;

            DataSet ds = new DataSet();

            ds = PhoenixCrewTravelVisa.CrewTravelVisaLineItemSearch(ViewState["TRAVELVISAID"] != null ? General.GetNullableGuid(ViewState["TRAVELVISAID"].ToString()) : null, sortexpression, sortdirection,
                                (int)ViewState["PAGENUMBER"],
                                gvVisaLineItem.PageSize,
                                ref iRowCount,
                                ref iTotalPageCount);

            gvVisaLineItem.DataSource = ds;
            gvVisaLineItem.VirtualItemCount = iRowCount;
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["RECORDSCOUNT"] = "1";
            }

            General.SetPrintOptions("gvVisaLineItem", "Seafarer Details", alCaptions, alColumns, ds);

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            MainMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVisaLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVisaLineItem.CurrentPageIndex + 1;

        BindData();
    }


    protected void gvVisaLineItem_ItemCommand(object sender, GridCommandEventArgs e)
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

    protected void gvVisaLineItem_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblVisaLineItemId = (RadLabel)e.Item.FindControl("lblVisaLineItemId");

            if (lblVisaLineItemId != null)
            {
                PhoenixCrewTravelVisa.DeleteTravelVisaLineItem(General.GetNullableGuid(lblVisaLineItemId.Text.ToString()));                             
                ListTravelVisaRequest(new Guid(ViewState["TRAVELVISAID"].ToString()));
            }
            BindData();
            gvVisaLineItem.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVisaLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            string travelvisaid = ((RadLabel)e.Item.FindControl("lblTravelVisaId")).Text;
            string visalineitemid = ((RadLabel)e.Item.FindControl("lblVisaLineItemId")).Text;

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
                cme.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewTravelVisaLineItemDetails.aspx?travelvisaid=" + travelvisaid + "&visalineitemid=" + visalineitemid + "');return false;");
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs args)
    {
        gvVisaLineItem.Rebind();
    }

    private bool IsValidTravelVisaRequest()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
        {
            ucError.ErrorMessage = "Vessel required";
        }
        if (General.GetNullableDateTime(txtVisaDate.Text) == null)
        {
            ucError.ErrorMessage = "Date required";
        }
        if (General.GetNullableInteger(ucMultiAgent.SelectedValue) == null)
        {
            ucError.ErrorMessage = "Agent required";
        }
        if (General.GetNullableInteger(ucCountry.SelectedCountry) == null)
        {
            ucError.ErrorMessage = "Visa country required";
        }
        if (General.GetNullableString(txtRemarks.Text) == null)
        {
            ucError.ErrorMessage = "Remarks required";
        }
        return (!ucError.IsError);
    }


}
