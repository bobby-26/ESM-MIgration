using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionPurchaseForm : PhoenixBasePage
{
    string vesselname;
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
        SetTabHighlight();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
                    Response.Redirect("PhoenixLogout.aspx");
                VesselConfiguration();
                Session["New"] = "N";

                if (Request.QueryString["pageno"] != null)
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["pageno"].ToString());
                else
                    ViewState["PAGENUMBER"] = 1;

                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["orderid"] = null;
                ViewState["PAGEURL"] = null;
                ViewState["RecordAndResponseId"] = null;

                if (Request.QueryString["RecordResponseId"] != null && Request.QueryString["RecordResponseId"].ToString() != "")
                    ViewState["RecordAndResponseId"] = Request.QueryString["RecordResponseId"].ToString();

                if (Request.QueryString["CHECKLISTID"] != null)
                    ViewState["CHECKLISTID"] = Request.QueryString["CHECKLISTID"];
                else
                    ViewState["CHECKLISTID"] = null;

                if (Request.QueryString["orderid"] != null && Request.QueryString["orderid"].ToString() != "")
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Inspection/InspectionPurchaseFormGeneral.aspx?orderid=" + Request.QueryString["orderid"].ToString() + "&reffrom=" + ViewState["REFFROM"];
                    lblorderId.Text = ViewState["orderid"].ToString();
                }
                gvFormDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            SessionUtil.PageAccessRights(this.ViewState);
            ViewState["DIRECTOBJ"] = null;
            if (Request.QueryString["DIRECTOBJ"] != null)
            {
                ViewState["DIRECTOBJ"] = Request.QueryString["DIRECTOBJ"];
            }
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            lblorderId.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionPurchaseForm.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvFormDetails')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbargrid.AddImageButton("../Purchase/PurchaseForm.aspx", "Copy Requisition", "Copy.png", "COPY");
            
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //MenuOrderForm.SetTrigger(pnlOrderForm);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (Request.QueryString["reffrom"] != null && Request.QueryString["reffrom"].ToString() == "directobs")
            {
                ViewState["REFFROM"] = Request.QueryString["reffrom"].ToString();
                toolbarmain.AddButton("Back", "DIRECTOBSERVATION", ToolBarDirection.Right);
            }
            else
            {
                ViewState["REFFROM"] = "";
                toolbarmain.AddButton("Observation", "OBSERVATION", ToolBarDirection.Right);
            }
            toolbarmain.AddButton("Line Items", "LINEITEM", ToolBarDirection.Right);
            toolbarmain.AddButton("Form", "FORM", ToolBarDirection.Right);
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            //MenuOrderFormMain.SetTrigger(pnlOrderForm);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FORM"))
            {
                if (ViewState["orderid"] != null)
                {
                    ViewState["PAGEURL"] = null;
                    ViewState["PAGEURL"] = "../Inspection/InspectionPurchaseFormGeneral.aspx?DIRECTOBJ=" + ViewState["DIRECTOBJ"];
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "&orderid=" + ViewState["orderid"] + "&RecordResponseId=" + ViewState["RecordAndResponseId"] + "&reffrom=" + ViewState["REFFROM"];
                }
            }            
            else if (CommandName.ToUpper().Equals("OBSERVATION"))
            {
                Response.Redirect("../Inspection/InspectionObservationMaster.aspx?currenttab=observation&OBSID=" + ViewState["DIRECTOBJ"] + "&RECORDRESPONSEID=" + ViewState["RecordAndResponseId"] + "&CHECKLISTID=" + ViewState["CHECKLISTID"], false);
            }
            else if (CommandName.ToUpper().Equals("DIRECTOBSERVATION"))
            {
                Response.Redirect("../Inspection/InspectionDirectObservationMaster.aspx?currenttab=" + Request.QueryString["currenttab"] + "&OBSID=" + ViewState["DIRECTOBJ"] + "&DIRECTOBSERVATIONID=" + ViewState["DIRECTOBJ"], false);
            }
            else if (CommandName.ToUpper().Equals("LINEITEM"))
            {
                if (ViewState["orderid"] != null)
                {                                       
                    ViewState["PAGEURL"] = "../Inspection/InspectionPurchaseFormLineItem.aspx?DIRECTOBJ=" + ViewState["DIRECTOBJ"];
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["PAGENUMBER"] + "&RecordResponseId=" + ViewState["RecordAndResponseId"] + "&reffrom=" + ViewState["REFFROM"].ToString();
                }
            }
            else
            {
                if (ViewState["RecordAndResponseId"] != null)
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"] + "&RecordResponseId=" + ViewState["RecordAndResponseId"].ToString() + "&reffrom=" + ViewState["REFFROM"].ToString();
                else
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?orderid=" + ViewState["orderid"] + "&RecordResponseId=null" + "&reffrom=" + ViewState["REFFROM"].ToString();
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
        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "FLDVENDORNAME", "FLDFORMTYPENAME", "FLDFORMSTATUSNAME", "FLDSUBACCOUNT", "FLDVENDORDELIVERYDATE", "FLDSTOCKTYPE", "FLDSTOCKCLASS", "FLDVESSELNAME" };
        string[] alCaptions = { "Number", "Form Title", "Vendor", "Form Type", "Form Status", "Budget Code", "Received Date", "Type", "Component Class/Store Type", "Vessel" };
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionPurchaseForm.OrderFormSearch(null,
              ViewState["DIRECTOBJ"] != null ? General.GetNullableGuid(ViewState["DIRECTOBJ"].ToString()) : null,
               null,
               sortexpression, 
               sortdirection, 
               1, 
               iRowCount,
               ref iRowCount, 
               ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OrderForm.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Purchase order form - " + vesselname + "</center></h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("COPY"))
            {
                //ucConfirm.Visible = true;
                //ucConfirm.Text = " Do you want to copy Form " + PhoenixInspectionPurchaseForm.FormNumber + "? ";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void CopyForm_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
    //        if (ucCM.confirmboxvalue == 1)
    //        {
    //            GetDocumentNumber();
                //PhoenixPurchaseOrderForm.CopytOrderFormRequisition(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //              , new Guid(ViewState["orderid"].ToString())
                //              , Filter.CurrentInspectionPurchaseVesselSelection
                //              , Int32.Parse(ViewState["DocumentNumber"].ToString())
                //              , Filter.CurrentInspectionPurchaseStockType);
    //            BindData();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "FLDVENDORNAME", "FLDFORMTYPENAME", "FLDFORMSTATUSNAME", "FLDSUBACCOUNT", "FLDVENDORDELIVERYDATE", "FLDSTOCKTYPE", "FLDSTOCKCLASS", "FLDVESSELNAME" };
        string[] alCaptions = { "Number", "Form Title", "Vendor", "Form Type", "Form Status", "Budget Code", "Received Date", "Type", "Component Class/Store Type", "Vessel" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());


        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixInspectionPurchaseForm.OrderFormSearch(null,
                                                                   ViewState["DIRECTOBJ"] != null ? General.GetNullableGuid(ViewState["DIRECTOBJ"].ToString()) : null,
                                                                   null,
                                                                   sortexpression,
                                                                   sortdirection,
                                                                   (int)ViewState["PAGENUMBER"],
                                                                   gvFormDetails.PageSize,
                                                                   ref iRowCount,
                                                                   ref iTotalPageCount);

        gvFormDetails.DataSource = ds;
        gvFormDetails.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            vesselname = dr["FLDVESSELNAME"].ToString();
            
            if (ViewState["orderid"] == null)
            {
                ViewState["orderid"] = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                lblorderId.Text = ViewState["orderid"].ToString();
                gvFormDetails.SelectedIndexes.Clear();
                MenuOrderFormMain.Visible = true;
                ViewState["PAGEURL"] = null;
            }

            if (ViewState["PAGEURL"] == null)
            {
                ViewState["PAGEURL"] = "../Inspection/InspectionPurchaseFormGeneral.aspx?DIRECTOBJ=" + ViewState["DIRECTOBJ"];
                if (ViewState["RecordAndResponseId"] != null)
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&RecordResponseId=" + ViewState["RecordAndResponseId"].ToString() + "&reffrom=" + ViewState["REFFROM"].ToString();
                else
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&RecordResponseId=null" + "&reffrom=" + ViewState["REFFROM"].ToString();
            }
            else if (ViewState["PAGEURL"].ToString().Contains("orderid"))
            {
                if (ViewState["RecordAndResponseId"] != null)
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "&RecordResponseId=" + ViewState["RecordAndResponseId"].ToString() + "&reffrom=" + ViewState["REFFROM"].ToString();
                else
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "&RecordResponseId=null" + "&reffrom=" + ViewState["REFFROM"].ToString();
            }
            else
            {
                if (ViewState["RecordAndResponseId"] != null)
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&RecordResponseId=" + ViewState["RecordAndResponseId"].ToString() + "&reffrom=" + ViewState["REFFROM"].ToString(); 
                else
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&RecordResponseId=null" + "&reffrom=" + ViewState["REFFROM"].ToString(); 
            }
            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ViewState["PAGEURL"] = "../Inspection/InspectionPurchaseFormType.aspx?DIRECTOBJ=" + ViewState["DIRECTOBJ"] + "&RecordResponseId=" + ViewState["RecordAndResponseId"] + "&reffrom=" + ViewState["REFFROM"];
            ifMoreInfo.Attributes["src"] = "InspectionPurchaseFormType.aspx?DIRECTOBJ=" + ViewState["DIRECTOBJ"] + "&RecordResponseId=" + ViewState["RecordAndResponseId"] + "&reffrom=" + ViewState["REFFROM"];

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (Request.QueryString["reffrom"] != null && Request.QueryString["reffrom"].ToString() == "directobs")
            {
                toolbarmain.AddButton("Back", "DIRECTOBSERVATION", ToolBarDirection.Right);
            }
            else
            {
                toolbarmain.AddButton("Observation", "OBSERVATION", ToolBarDirection.Right);
            }
            toolbarmain.AddButton("Line Items", "LINEITEM", ToolBarDirection.Right);
            toolbarmain.AddButton("Form", "FORM", ToolBarDirection.Right);
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            MenuOrderFormMain.SelectedMenuIndex = 2;
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvFormDetails", "Order Form List -  " + vesselname, alCaptions, alColumns, ds);
        SetTabHighlight();
    }

    private void SetRowSelection()
    {
        gvFormDetails.SelectedIndexes.Clear();
        if (ViewState["orderid"] != null && ViewState["orderid"].ToString() != "")
        {
            for (int i = 0; i < gvFormDetails.Items.Count; i++)
            {
                if (gvFormDetails.MasterTableView.Items[i].GetDataKeyValue("FLDORDERID").ToString().Equals(ViewState["orderid"].ToString()))
                {
                    PhoenixInspectionPurchaseForm.FormNumber = ((LinkButton)gvFormDetails.Items[i].FindControl("lnkFormNumberName")).Text;
                    Filter.CurrentInspectionPurchaseVesselSelection = int.Parse(((RadLabel)gvFormDetails.Items[i].FindControl("lblVesselID")).Text);
                    Filter.CurrentInspectionPurchaseStockType = ((RadLabel)gvFormDetails.Items[i].FindControl("lblStockType")).Text;
                    ViewState["DTKEY"] = ((RadLabel)gvFormDetails.Items[i].FindControl("lbldtkey")).Text;
                    Filter.CurrentInspectionPurchaseStockClass = ((RadLabel)gvFormDetails.Items[i].FindControl("lblStockId")).Text;
                    BindSendDate();
                    gvFormDetails.MasterTableView.Items[i].Selected = true;
                }
            }
        }
    }

    private void BindSendDate()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.OrderFormCheckSendToOffice(int.Parse(Filter.CurrentInspectionPurchaseVesselSelection.ToString()), new Guid(ViewState["orderid"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentInspectionPurchaseVesselSendDateSelection = ds.Tables[0].Rows[0]["FLDAUDITTRANSFERDATE"].ToString();
        else
            Filter.CurrentInspectionPurchaseVesselSendDateSelection = "";
    }
    protected void gvFormDetails_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblPriorityFlag = (RadLabel)e.Item.FindControl("lblPriorityFlag");

            LinkButton lnkFormNumberName = (LinkButton)e.Item.FindControl("lnkFormNumberName");
            Int64 result = 0;

            if (Int64.TryParse(lblPriorityFlag.Text, out result))
            {
                e.Item.ForeColor = (result == 1) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                lnkFormNumberName.ForeColor = (result == 1) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            }
        }
        //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit && e.Row.RowState != (DataControlRowState.Alternate | DataControlRowState.Edit))
        //{
        //    LinkButton _doubleClickButton = (LinkButton)e.Row.FindControl("lnkDoubleClick");
        //    string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
        //}
    }
    protected void gvFormDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "RowClick")
        {
            Filter.CurrentInspectionPurchaseStockType = ((RadLabel)e.Item.FindControl("lblStockType")).Text;
            BindPageURL(e.Item.ItemIndex);
        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            Filter.CurrentInspectionPurchaseStockType = ((RadLabel)e.Item.FindControl("lblStockType")).Text;
            BindPageURL(e.Item.ItemIndex);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
        if (Session["New"].ToString() == "Y")
        {
            gvFormDetails.SelectedIndexes.Clear();
            Session["New"] = "N";
            BindPageURL(0);
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblNumber = (RadLabel)gvFormDetails.Items[rowindex].FindControl("lblNumber");
            RadLabel lbldtkey = (RadLabel)gvFormDetails.Items[rowindex].FindControl("lbldtkey");

            if (lblNumber != null)
            {
                ViewState["orderid"] = lblNumber.Text;
                lblorderId.Text = ViewState["orderid"].ToString();
            }
            if(lbldtkey!=null)
                ViewState["DTKEY"] = lbldtkey.Text;

            //Filter.CurrentInspectionPurchaseVesselSelection = int.Parse(((RadLabel)gvFormDetails.Rows[rowindex].FindControl("lblVesselID")).Text);
            //PhoenixInspectionPurchaseForm.FormNumber = ((LinkButton)gvFormDetails.Rows[rowindex].FindControl("lnkFormNumberName")).Text;            

            if (ViewState["PAGEURL"].ToString().Contains("CommonFileAttachment.aspx"))
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PURCHASE + "&reffrom=" + ViewState["REFFROM"].ToString();
            }
            else
            {
                if (ViewState["RecordAndResponseId"] != null)
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "&orderid=" + ViewState["orderid"] + "&RecordResponseId=" + ViewState["RecordAndResponseId"].ToString() + "&reffrom=" + ViewState["REFFROM"].ToString();
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "&orderid=" + ViewState["orderid"] + "&RecordResponseId=null" + "&reffrom=" + ViewState["REFFROM"].ToString();
                }
            }
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SetTabHighlight()
    {
        try
        {
            if (ViewState["PAGEURL"].ToString().Trim().Contains("InspectionPurchaseFormGeneral.aspx"))
            {
                MenuOrderFormMain.SelectedMenuIndex = 2;
            }
            if (ViewState["PAGEURL"].ToString().Trim().Contains("InspectionPurchaseFormLineItem.aspx"))
            {
                MenuOrderFormMain.SelectedMenuIndex = 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
    private void GetDocumentNumber()
    {
        DataTable dt = PhoenixInspectionAuditPurchaseForm.GetDocumentNumber();
        if (dt.Rows.Count > 0)
        {
            ViewState["DocumentNumber"] = dt.Rows[0]["FLDDOCUMENTTYPEID"].ToString();
        }
        else
        {
            ViewState["DocumentNumber"] = "0";
        }
    }

    protected void gvFormDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFormDetails.CurrentPageIndex + 1;
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
        gvFormDetails.SelectedIndexes.Clear();
        gvFormDetails.EditIndexes.Clear();
        gvFormDetails.DataSource = null;
        gvFormDetails.Rebind();
    }
    protected void gvFormDetails_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
