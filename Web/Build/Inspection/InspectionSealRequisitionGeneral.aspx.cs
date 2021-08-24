using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class InspectionSealRequisitionGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionSealRequisitionGeneral.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSealReqLine')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuSealReqLineItem.AccessRights = this.ViewState;
            MenuSealReqLineItem.MenuList = toolbargrid.Show();
            //MenuSealReqLineItem.SetTrigger(pnlSealReq);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                ucConfirm.Visible = false;
               
                ViewState["REQUESTID"] = null;
                if (Request.QueryString["REQUESTID"] != null)
                    ViewState["REQUESTID"] = Request.QueryString["REQUESTID"];
                txtVesselName.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                if (ViewState["REQUESTID"] != null)
                    EditSealRequisition(new Guid(ViewState["REQUESTID"].ToString()));
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["REQSTATUS"] = "";
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar.AddButton("New", "NEW",ToolBarDirection.Right);
               
                MenuSealReq.AccessRights = this.ViewState;
                MenuSealReq.MenuList = toolbar.Show();

                gvSealReqLine.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            // BindData();
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
            string[] alColumns = { "FLDSEALTYPENAME", "FLDQUANTITY", "FLDISSUEDQUANTITY", "FLDRECEIVEDQTY", "FLDCANCELLEDQTY", "FLDRECEIVEDDATE" };
            string[] alCaptions = { "Seal Type", "Requested Qty", "Issued Qty from Office", "Received Qty", "Cancelled Qty", "Received Date" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            Guid? requestid = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["REQUESTID"] != null)
            {
                requestid = new Guid(ViewState["REQUESTID"].ToString());
            }

            DataSet ds = PhoenixInspectionSealRequisition.SearchSealRequesitionLineItem(requestid
                                      , sortexpression, sortdirection
                                      , int.Parse(ViewState["PAGENUMBER"].ToString())
                                      , gvSealReqLine.PageSize, ref iRowCount, ref iTotalPageCount);


            gvSealReqLine.DataSource = ds;
            gvSealReqLine.VirtualItemCount = iRowCount;
            General.SetPrintOptions("gvSealReqLine", "Line Item", alCaptions, alColumns, ds);
            //ViewState["ROWCOUNT"] = iRowCount;
            //ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            // SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSealReq_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRequest(txtRequestDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["REQUESTID"] == null)
                {
                    Guid? newinsertedid = null;
                    PhoenixInspectionSealRequisition.InsertSealRequesition(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                        DateTime.Parse(txtRequestDate.Text), ref newinsertedid);
                    Filter.CurrentSelectedSealRequisition = null;
                }
                else
                {
                    PhoenixInspectionSealRequisition.UpdateSealRequesition(new Guid(ViewState["REQUESTID"].ToString())
                        , DateTime.Parse(txtRequestDate.Text));
                    EditSealRequisition(new Guid(ViewState["REQUESTID"].ToString()));
                }
                String script = String.Format("javascript:fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                BindData();
                gvSealReqLine.Rebind();
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["REQUESTID"] = null;
                ViewState["REQUISITIONSTATUS"] = null;
                txtVesselName.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                txtRefNo.Text = string.Empty;
                txtRequestDate.Text = string.Empty;
                txtRequestDate.Enabled = true;
                BindData();
                gvSealReqLine.Rebind();
                //SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditSealRequisition(Guid gRequestId)
    {
        try
        {
            DataTable dt = PhoenixInspectionSealRequisition.EditSealRequesition(gRequestId);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
                txtRefNo.Text = dr["FLDREFERENCENO"].ToString();
                txtRequestDate.Text = dr["FLDREQUESTDATE"].ToString();
                ViewState["REQUISITIONSTATUS"] = dr["FLDREQUESTSTATUS"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRequest(string requestdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(requestdate).HasValue)
        {
            ucError.ErrorMessage = "Request Date is required.";
        }
        else if (DateTime.TryParse(requestdate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Request Date should not be future date.";
        }
        if (ViewState["REQUESTID"] == null)
            ucError.ErrorMessage = "Requisition can not be created without line items.";

        return (!ucError.IsError);
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSEALTYPENAME", "FLDQUANTITY", "FLDISSUEDQUANTITY", "FLDRECEIVEDQTY", "FLDCANCELLEDQTY", "FLDRECEIVEDDATE" };
            string[] alCaptions = { "Seal Type", "Requested Qty", "Issued Qty from Office", "Received Qty", "Cancelled Qty", "Received Date" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionSealRequisition.SearchSealRequesitionLineItem(new Guid(ViewState["REQUESTID"].ToString())
                                      , sortexpression, sortdirection
                                      , 1
                                      , iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Line Item", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuSealReqLineItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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


    //protected void gvSealReqLine_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            if (ViewState["SORTEXPRESSION"] != null)
    //            {
    //                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //                if (img != null)
    //                {
    //                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                        img.Src = Session["images"] + "/arrowUp.png";
    //                    else
    //                        img.Src = Session["images"] + "/arrowDown.png";

    //                    img.Visible = true;
    //                }
    //            }
    //        }
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //            ImageButton ab = (ImageButton)e.Row.FindControl("cmdConfirm");
    //            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
    //            UserControlQuick q = ((UserControlQuick)e.Row.FindControl("ddlSealTypeEdit"));
    //            if (q != null) q.bind();

    //            if (db != null)
    //            {
    //                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //            }                
    //            if (ab != null)
    //            {
    //                ab.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm the receipt?')");
    //                ab.Visible = SessionUtil.CanAccess(this.ViewState, ab.CommandName);
    //            }

    //            UserControlDate txtReceivedDateEdit = (UserControlDate)e.Row.FindControl("txtReceivedDateEdit");
    //            if (drv["FLDREQUESTSTATUSID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "REQ")
    //                || drv["FLDREQUESTSTATUSID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "ISS")
    //                || drv["FLDREQUESTSTATUSID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "REC"))
    //            {
    //                if (txtReceivedDateEdit != null) txtReceivedDateEdit.Enabled = true;
    //                //if (ab != null) ab.Visible = true;
    //            }
    //            else
    //            {
    //                if (txtReceivedDateEdit != null) txtReceivedDateEdit.Enabled = false;
    //                //if (ab != null) ab.Visible = false;
    //            }

    //            if (!String.IsNullOrEmpty(drv["FLDREQUESTSTATUSID"].ToString()))
    //            {
    //                ImageButton sn = (ImageButton)e.Row.FindControl("cmdIssue");
    //                UserControlMaskNumber txtQty = (UserControlMaskNumber)e.Row.FindControl("txtQuantityEdit");
    //                UserControlQuick ddlSealTypeEdit = (UserControlQuick)e.Row.FindControl("ddlSealTypeEdit");
    //                Label lblRequestlineId = (Label)e.Row.FindControl("lblRequestlineId");
    //                ImageButton rs = (ImageButton)e.Row.FindControl("cmdReceive");

    //                if (sn != null && lblRequestlineId != null)
    //                {
    //                    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
    //                        sn.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','', '../Inspection/InspectionSealNumberRecording.aspx?REQUESTLINEID=" + lblRequestlineId.Text + "',null)");
    //                    //else if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
    //                    //    sn.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','', '../Inspection/InspectionSealNumberIssued.aspx?REQUESTLINEID=" + lblRequestlineId.Text + "',null)");
    //                }

    //                if (rs != null && lblRequestlineId != null)
    //                    rs.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','', '../Inspection/InspectionSealsReceive.aspx?ISCONFIRMED=" + drv["FLDACTIVEYN"].ToString() + "&REQUESTLINEID=" + lblRequestlineId.Text + "',null)");

    //                ViewState["REQSTATUS"] = drv["FLDREQUESTSTATUSID"].ToString();
    //                if (drv["FLDREQUESTSTATUSID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "REQ"))
    //                    //|| drv["FLDREQUESTSTATUSID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "ISS")
    //                    //|| drv["FLDREQUESTSTATUSID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "REC"))
    //                {
    //                    if (txtQty != null) txtQty.ReadOnly = true;
    //                    if (ddlSealTypeEdit != null) ddlSealTypeEdit.Enabled = false;
    //                    if (sn != null)
    //                    {
    //                        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
    //                            sn.Visible = true;
    //                        else
    //                            sn.Visible = false;
    //                    }
    //                    if (txtReceivedDateEdit != null) txtReceivedDateEdit.Visible = true;
    //                    if (eb != null) eb.Visible = false;
    //                    if (db != null) db.Visible = false;
    //                    if (ab != null) ab.Visible = true;
    //                    if (rs != null)
    //                    {
    //                        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
    //                            rs.Visible = true;
    //                        else
    //                            rs.Visible = false;
    //                    }
    //                }
    //                else if (drv["FLDREQUESTSTATUSID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "ISS")
    //                        || drv["FLDREQUESTSTATUSID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "REC"))
    //                {
    //                    if (sn != null)
    //                    {
    //                        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
    //                            sn.Visible = true;
    //                        else
    //                            sn.Visible = false;
    //                    }
    //                    if (rs != null)
    //                    {
    //                        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
    //                            rs.Visible = true;
    //                        else
    //                            rs.Visible = false;
    //                    }

    //                    if (txtReceivedDateEdit != null) txtReceivedDateEdit.Visible = true;
    //                    if (eb != null) eb.Visible = true;                        
    //                    if (txtQty != null) txtQty.ReadOnly = true;
    //                    if (ddlSealTypeEdit != null) ddlSealTypeEdit.Enabled = false;
    //                    if (db != null) db.Visible = false;
    //                    if (ab != null) ab.Visible = false;
    //                }

    //                else if (drv["FLDREQUESTSTATUSID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "PEN"))
    //                {
    //                    if (sn != null) sn.Visible = false;
    //                    if (rs != null) rs.Visible = false;
    //                    if (txtReceivedDateEdit != null) txtReceivedDateEdit.Visible = false;
    //                    if (txtQty != null) txtQty.ReadOnly = false;
    //                    if (ddlSealTypeEdit != null) ddlSealTypeEdit.Enabled = true;
    //                    if (db != null) db.Visible = true;
    //                    if (eb != null) eb.Visible = true;
    //                    if (ab != null) ab.Visible = false;
    //                }
    //                else if (drv["FLDREQUESTSTATUSID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "CAN"))
    //                {
    //                    if (txtQty != null) txtQty.ReadOnly = true;
    //                    if (ddlSealTypeEdit != null) ddlSealTypeEdit.Enabled = false;
    //                    if (sn != null) sn.Visible = false;
    //                    if (txtReceivedDateEdit != null) txtReceivedDateEdit.Visible = false;
    //                    if (eb != null) eb.Visible = false;
    //                    if (db != null) db.Visible = false;
    //                    if (ab != null) ab.Visible = false;
    //                    if (rs != null) rs.Visible = false;
    //                }

    //                //if (drv["FLDACTIVEYN"].ToString() == "0")
    //                //{
    //                //    if (eb != null) eb.Visible = false;
    //                //    if (ab != null) ab.Visible = false;
    //                //}
    //                if (sn != null)
    //                {
    //                    if (!SessionUtil.CanAccess(this.ViewState, sn.CommandName)) sn.Visible = false;
    //                }
    //                if (rs != null)
    //                {
    //                    if (!SessionUtil.CanAccess(this.ViewState, rs.CommandName)) rs.Visible = false;
    //                }
    //            }

    //            if (eb != null)
    //            {
    //                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName)) eb.Visible = false;
    //            }
    //            if (db != null)
    //            {
    //                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
    //            }
    //            if (ab != null)
    //            {
    //                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName)) ab.Visible = false;
    //            }
    //        }
    //        if (e.Row.RowType == DataControlRowType.Footer)
    //        {
    //            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
    //            if (ab != null)
    //            {                    
    //                if (!string.IsNullOrEmpty(ViewState["REQSTATUS"].ToString()))
    //                {
    //                    if (ViewState["REQSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "PEN"))
    //                    {
    //                        if (ab != null) ab.Visible = true;
    //                    }
    //                    else
    //                    {
    //                        if (ab != null) ab.Visible = false;
    //                    }
    //                    if (ViewState["REQSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "CAN"))
    //                    {
    //                        if (ab != null) ab.Visible = false;
    //                    }
    //                }
    //                if (ViewState["REQUESTID"] == null)
    //                {
    //                    if (ab != null) ab.Visible = true;
    //                }
    //                if (ViewState["REQUISITIONSTATUS"] != null && ViewState["REQUISITIONSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "CAN"))
    //                {
    //                    if (ab != null) ab.Visible = false;
    //                }
    //                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName)) ab.Visible = false;
    //            }
    //            UserControlQuick qa = ((UserControlQuick)e.Row.FindControl("ddlSealTypeAdd"));
    //            if (qa != null) qa.bind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}



    //protected void gvSealReqLine_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            if (!IsValidRequest(
    //                (((UserControlQuick)_gridView.FooterRow.FindControl("ddlSealTypeAdd")).SelectedQuick),
    //                (((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtQuantity")).Text),
    //                txtRequestDate.Text))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            if (ViewState["REQUESTID"] == null)
    //            {
    //                Guid? newinsertedid = null;
    //                PhoenixInspectionSealRequisition.InsertSealRequesition(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
    //                    DateTime.Parse(txtRequestDate.Text), ref newinsertedid);
    //                Filter.CurrentSelectedSealRequisition = null;
    //                ViewState["REQUESTID"] = newinsertedid.ToString();
    //                EditSealRequisition(new Guid(ViewState["REQUESTID"].ToString()));
    //                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
    //            }
    //            PhoenixInspectionSealRequisition.InsertSealRequisitionLineItem(new Guid(ViewState["REQUESTID"].ToString())
    //                                , int.Parse(((UserControlQuick)_gridView.FooterRow.FindControl("ddlSealTypeAdd")).SelectedQuick)
    //                                , int.Parse(((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtQuantity")).Text));
    //            BindData();
    //           // SetPageNavigator();
    //            ucStatus.Text = "Line item is added successfully.";

    //        }
    //        if (e.CommandName.ToUpper().Equals("UPDATELINEITEM"))
    //        {

    //            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //            string requestlineid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestLineIdEdit")).Text;
    //            UserControlMaskNumber txtQty = (UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantityEdit");
    //            UserControlQuick sealtype = (UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ddlSealTypeEdit");
    //            UserControlDate txtReceivedDateEdit = (UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtReceivedDateEdit");
    //            string statusid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStatusIdEdit")).Text;

    //            ImageButton ib = (ImageButton)_gridView.Rows[nCurrentRow].FindControl("cmdReceive");
    //            if (statusid != null && (statusid == PhoenixCommonRegisters.GetHardCode(1, 196, "ISS") || statusid == PhoenixCommonRegisters.GetHardCode(1, 196, "REC")))
    //            {
    //                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
    //                {
    //                    ucError.ErrorMessage = "Only vessel can update the received date.";
    //                    ucError.Visible = true;
    //                    return;
    //                }
    //                if (!IsValidReceivedDate(txtReceivedDateEdit.Text))
    //                {
    //                    ucError.Visible = true;
    //                    return;
    //                }
    //                PhoenixInspectionSealRequisition.UpdateReceivedDate(new Guid(requestlineid), DateTime.Parse(txtReceivedDateEdit.Text));
    //                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
    //            }
    //            else
    //            {
    //                if (!IsValidRequest(sealtype.SelectedQuick, txtQty.Text, txtRequestDate.Text))
    //                {
    //                    ucError.Visible = true;
    //                    return;
    //                }
    //                int qty = int.Parse(txtQty.Text.Trim());
    //                PhoenixInspectionSealRequisition.UpdateSealRequesitionLineItem(new Guid(requestlineid), int.Parse(sealtype.SelectedQuick), qty);
    //            }
    //            _gridView.EditIndex = -1;
    //            BindData();
    //           // SetPageNavigator();
    //            ucStatus.Text = "Line item is updated successfully.";
    //        }
    //        if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
    //            ViewState["DELETEID"] = id.ToString();
    //            int result = 0;
    //            PhoenixInspectionSealRequisition.CheckIfSealRequisitionCancel(new Guid(ViewState["REQUESTID"].ToString()), ref result);
    //            if (result == 1)
    //            {
    //                ucConfirm.Visible = true;
    //                ucConfirm.Text = "Do you want to cancel the seal requisition?";
    //                return;
    //            }
    //            else
    //                PhoenixInspectionSealRequisition.DeleteSealRequesitionLineItem(id);

    //            _gridView.EditIndex = -1;
    //            BindData();
    //            //SetPageNavigator();
    //            ucStatus.Text = "Line item is deleted successfully.";
    //        }
    //        else if (e.CommandName.ToUpper().Equals("EDIT"))
    //        {
    //            _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
    //        }
    //        else if (e.CommandName.ToUpper().Equals("CONFIRM"))
    //        {
    //            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
    //            {
    //                ucError.ErrorMessage = "Only vessel can confirm the receipt.";
    //                ucError.Visible = true;
    //                return;
    //            }
    //            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //            string requestlineid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestlineId")).Text;
    //            PhoenixInspectionSealRequisition.SealReqLineItemConfirmReceipt(new Guid(requestlineid));
    //            ucStatus.Text = "Receipt is confirmed successfully.";
    //            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}





    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            // SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    try
    //    {
    //        gvSealReqLine.SelectedIndex = -1;
    //        gvSealReqLine.EditIndex = -1;
    //        if (ce.CommandName == "prev")
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //        else
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //        ViewState["REQUESTLINEID"] = null;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private void SetPageNavigator()
    //{
    //    try
    //    {
    //        cmdPrevious.Enabled = IsPreviousEnabled();
    //        cmdNext.Enabled = IsNextEnabled();
    //        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //        return true;

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    try
    //    {
    //        dt.Rows.Add(dt.NewRow());
    //        gv.DataSource = dt;
    //        gv.DataBind();

    //        int colcount = gv.Columns.Count;
    //        gv.Rows[0].Cells.Clear();
    //        gv.Rows[0].Cells.Add(new TableCell());
    //        gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //        gv.Rows[0].Cells[0].Font.Bold = true;
    //        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //        gv.Rows[0].Attributes["onclick"] = "";
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    private bool IsValidRequest(string sealtype, string qty, string requestdate)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        DateTime resultDate;
        if (!General.GetNullableDateTime(requestdate).HasValue)
        {
            ucError.ErrorMessage = "Request Date is required.";
        }
        else if (DateTime.TryParse(requestdate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Request Date should not be future date.";
        }
        if (String.IsNullOrEmpty(sealtype) || sealtype.ToUpper().Equals("DUMMY"))
        {
            ucError.ErrorMessage = "Seal type is Required.";
        }
        if (String.IsNullOrEmpty(qty))
        {
            ucError.ErrorMessage = "Quantity is Required.";
        }
        else if (int.Parse(qty) <= 0)
        {
            ucError.ErrorMessage = "Quantity should be greater than Zero.";
        }

        return (!ucError.IsError);
    }

    private bool IsValidReceivedDate(string receiveddate)
    {
        ucError.HeaderMessage = "Please provide the following required information to Confirm the receipt.";
        DateTime resultDate;
        if (!General.GetNullableDateTime(receiveddate).HasValue)
        {
            ucError.ErrorMessage = "Received Date is required.";
        }
        else if (DateTime.TryParse(receiveddate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Received Date should not be future date.";
        }

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            ////gvSealReqLine.EditIndex = -1;
            ////gvSealReqLine.SelectedIndex = -1;
            gvSealReqLine.Rebind();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                PhoenixInspectionSealRequisition.DeleteSealRequesitionLineItem(new Guid(ViewState["DELETEID"].ToString()));
                PhoenixInspectionSealRequisition.CancelSealRequisition(new Guid(ViewState["REQUESTID"].ToString()));
                ucStatus.Text = "Seal requisition cancelled successfully.";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealReqLine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSealReqLine.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealReqLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidRequest(
                    (((UserControlQuick)e.Item.FindControl("ddlSealTypeAdd")).SelectedQuick),
                    (((UserControlMaskNumber)e.Item.FindControl("txtQuantity")).Text),
                    txtRequestDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["REQUESTID"] == null)
                {
                    Guid? newinsertedid = null;
                    PhoenixInspectionSealRequisition.InsertSealRequesition(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                        DateTime.Parse(txtRequestDate.Text), ref newinsertedid);
                    Filter.CurrentSelectedSealRequisition = null;
                    ViewState["REQUESTID"] = newinsertedid.ToString();
                    EditSealRequisition(new Guid(ViewState["REQUESTID"].ToString()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
                }
                PhoenixInspectionSealRequisition.InsertSealRequisitionLineItem(new Guid(ViewState["REQUESTID"].ToString())
                                    , int.Parse(((UserControlQuick)e.Item.FindControl("ddlSealTypeAdd")).SelectedQuick)
                                    , int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtQuantity")).Text));
                BindData();
                gvSealReqLine.Rebind();
                // SetPageNavigator();
                ucStatus.Text = "Line item is added successfully.";


            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string requestlineid = ((RadLabel)e.Item.FindControl("lblRequestLineIdEdit")).Text;
                UserControlMaskNumber txtQty = (UserControlMaskNumber)e.Item.FindControl("txtQuantityEdit");
                UserControlQuick sealtype = (UserControlQuick)e.Item.FindControl("ddlSealTypeEdit");
                UserControlDate txtReceivedDateEdit = (UserControlDate)e.Item.FindControl("txtReceivedDateEdit");
                string statusid = ((RadLabel)e.Item.FindControl("lblStatusIdEdit")).Text;

                LinkButton ib = (LinkButton)e.Item.FindControl("cmdReceive");
                if (statusid != null && (statusid == PhoenixCommonRegisters.GetHardCode(1, 196, "ISS") || statusid == PhoenixCommonRegisters.GetHardCode(1, 196, "REC")))
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    {
                        ucError.ErrorMessage = "Only vessel can update the received date.";
                        ucError.Visible = true;
                        return;
                    }
                    if (!IsValidReceivedDate(txtReceivedDateEdit.Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionSealRequisition.UpdateReceivedDate(new Guid(requestlineid), DateTime.Parse(txtReceivedDateEdit.Text));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
                    BindData();
                    gvSealReqLine.Rebind();
                }
                else
                {
                    if (!IsValidRequest(sealtype.SelectedQuick, txtQty.Text, txtRequestDate.Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    int qty = int.Parse(txtQty.Text.Trim());
                    PhoenixInspectionSealRequisition.UpdateSealRequesitionLineItem(new Guid(requestlineid), int.Parse(sealtype.SelectedQuick), qty);
                }
                //_gridView.EditIndex = -1;
                BindData();
                gvSealReqLine.Rebind();
                // SetPageNavigator();
                ucStatus.Text = "Line item is updated successfully.";
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;



                Guid id = (Guid)eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDREQUESTLINEID"];
                ViewState["DELETEID"] = id.ToString();
                int result = 0;
                PhoenixInspectionSealRequisition.CheckIfSealRequisitionCancel(new Guid(ViewState["REQUESTID"].ToString()), ref result);
                if (result == 1)
                {
                    ucConfirm.Visible = true;
                    ucConfirm.Text = "Do you want to cancel the seal requisition?";
                    return;
                }
                else
                    PhoenixInspectionSealRequisition.DeleteSealRequesitionLineItem(id);


                 BindData();
                gvSealReqLine.Rebind();
                //SetPageNavigator();
                ucStatus.Text = "Line item is deleted successfully.";
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                // _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
            }
            else if (e.CommandName.ToUpper().Equals("CONFIRM"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                {
                    ucError.ErrorMessage = "Only vessel can confirm the receipt.";
                    ucError.Visible = true;
                    return;
                }

                string requestlineid = ((RadLabel)e.Item.FindControl("lblRequestlineId")).Text;
                PhoenixInspectionSealRequisition.SealReqLineItemConfirmReceipt(new Guid(requestlineid));
                ucStatus.Text = "Receipt is confirmed successfully.";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
            }
            else if (e.CommandName == "Page")
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

   

    protected void gvSealReqLine_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {


            if (e.Item is GridDataItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                LinkButton ab = (LinkButton)e.Item.FindControl("cmdConfirm");
                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                UserControlQuick q = ((UserControlQuick)e.Item.FindControl("ddlSealTypeEdit"));
                if (q != null) q.bind();

                if (db != null)
                {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
                if (ab != null)
                {
                    ab.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm the receipt?')");
                    ab.Visible = SessionUtil.CanAccess(this.ViewState, ab.CommandName);
                }

                UserControlDate txtReceivedDateEdit = (UserControlDate)e.Item.FindControl("txtReceivedDateEdit");
                if (DataBinder.Eval(e.Item.DataItem, "FLDREQUESTSTATUSID").ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "REQ")
                    || DataBinder.Eval(e.Item.DataItem, "FLDREQUESTSTATUSID").ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "ISS")
                    || DataBinder.Eval(e.Item.DataItem, "FLDREQUESTSTATUSID").ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "REC"))
                {
                    if (txtReceivedDateEdit != null) txtReceivedDateEdit.Enabled = true;
                    //if (ab != null) ab.Visible = true;
                }
                else
                {
                    if (txtReceivedDateEdit != null) txtReceivedDateEdit.Enabled = false;
                    //if (ab != null) ab.Visible = false;
                }

                if (!String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "FLDREQUESTSTATUSID").ToString()))
                {
                    LinkButton sn = (LinkButton)e.Item.FindControl("cmdIssue");
                    UserControlMaskNumber txtQty = (UserControlMaskNumber)e.Item.FindControl("txtQuantityEdit");
                    UserControlQuick ddlSealTypeEdit = (UserControlQuick)e.Item.FindControl("ddlSealTypeEdit");
                    RadLabel lblRequestlineId = (RadLabel)e.Item.FindControl("lblRequestlineId");
                    LinkButton rs = (LinkButton)e.Item.FindControl("cmdReceive");

                    if (sn != null && lblRequestlineId != null)
                    {
                        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                            sn.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionSealNumberRecording.aspx?REQUESTLINEID=" + lblRequestlineId.Text + "',null)");
                        //else if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                        //    sn.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','', '../Inspection/InspectionSealNumberIssued.aspx?REQUESTLINEID=" + lblRequestlineId.Text + "',null)");
                    }

                    if (rs != null && lblRequestlineId != null)
                        rs.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionSealsReceive.aspx?ISCONFIRMED=" + DataBinder.Eval(e.Item.DataItem, "FLDACTIVEYN").ToString() + "&REQUESTLINEID=" + lblRequestlineId.Text + "',null);return false;");

                    ViewState["REQSTATUS"] = DataBinder.Eval(e.Item.DataItem, "FLDREQUESTSTATUSID").ToString();
                    if (DataBinder.Eval(e.Item.DataItem, "FLDREQUESTSTATUSID").ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "REQ"))
                    //|| drv["FLDREQUESTSTATUSID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "ISS")
                    //|| drv["FLDREQUESTSTATUSID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "REC"))
                    {
                        if (txtQty != null) txtQty.ReadOnly = true;
                        if (ddlSealTypeEdit != null) ddlSealTypeEdit.Enabled = false;
                        if (sn != null)
                        {
                            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                                sn.Visible = true;
                            else
                                sn.Visible = false;
                        }
                        if (txtReceivedDateEdit != null) txtReceivedDateEdit.Visible = true;
                        if (eb != null) eb.Visible = false;
                        if (db != null) db.Visible = false;
                        if (ab != null) ab.Visible = true;
                        if (rs != null)
                        {
                            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                                rs.Visible = true;
                            else
                                rs.Visible = false;
                        }
                    }
                    else if (DataBinder.Eval(e.Item.DataItem, "FLDREQUESTSTATUSID").ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "ISS")
                            || DataBinder.Eval(e.Item.DataItem, "FLDREQUESTSTATUSID").ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "REC"))
                    {
                        if (sn != null)
                        {
                            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                                sn.Visible = true;
                            else
                                sn.Visible = false;
                        }
                        if (rs != null)
                        {
                            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                                rs.Visible = true;
                            else
                                rs.Visible = false;
                        }

                        if (txtReceivedDateEdit != null) txtReceivedDateEdit.Visible = true;
                        if (eb != null) eb.Visible = true;
                        if (txtQty != null) txtQty.ReadOnly = true;
                        if (ddlSealTypeEdit != null) ddlSealTypeEdit.Enabled = false;
                        if (db != null) db.Visible = false;
                        if (ab != null) ab.Visible = false;
                    }

                    else if (DataBinder.Eval(e.Item.DataItem, "FLDREQUESTSTATUSID").ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "PEN"))
                    {
                        if (sn != null) sn.Visible = false;
                        if (rs != null) rs.Visible = false;
                        if (txtReceivedDateEdit != null) txtReceivedDateEdit.Visible = false;
                        if (txtQty != null) txtQty.ReadOnly = false;
                        if (ddlSealTypeEdit != null) ddlSealTypeEdit.Enabled = true;
                        if (db != null) db.Visible = true;
                        if (eb != null) eb.Visible = true;
                        if (ab != null) ab.Visible = false;
                    }
                    else if (DataBinder.Eval(e.Item.DataItem, "FLDREQUESTSTATUSID").ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "CAN"))
                    {
                        if (txtQty != null) txtQty.ReadOnly = true;
                        if (ddlSealTypeEdit != null) ddlSealTypeEdit.Enabled = false;
                        if (sn != null) sn.Visible = false;
                        if (txtReceivedDateEdit != null) txtReceivedDateEdit.Visible = false;
                        if (eb != null) eb.Visible = false;
                        if (db != null) db.Visible = false;
                        if (ab != null) ab.Visible = false;
                        if (rs != null) rs.Visible = false;
                    }

                    //if (drv["FLDACTIVEYN"].ToString() == "0")
                    //{
                    //    if (eb != null) eb.Visible = false;
                    //    if (ab != null) ab.Visible = false;
                    //}
                    if (sn != null)
                    {
                        if (!SessionUtil.CanAccess(this.ViewState, sn.CommandName)) sn.Visible = false;
                    }
                    if (rs != null)
                    {
                        if (!SessionUtil.CanAccess(this.ViewState, rs.CommandName)) rs.Visible = false;
                    }
                }

                if (eb != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName)) eb.Visible = false;
                }
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }
                if (ab != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName)) ab.Visible = false;
                }
            }
            if (e.Item is GridFooterItem)
            {
                LinkButton ab = (LinkButton)e.Item.FindControl("cmdAdd");
                if (ab != null)
                {
                    if (!string.IsNullOrEmpty(ViewState["REQSTATUS"].ToString()))
                    {
                        if (ViewState["REQSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "PEN"))
                        {
                            if (ab != null) ab.Visible = true;
                        }
                        else
                        {
                            if (ab != null) ab.Visible = false;
                        }
                        if (ViewState["REQSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "CAN"))
                        {
                            if (ab != null) ab.Visible = false;
                        }
                    }
                    if (ViewState["REQUESTID"] == null)
                    {
                        if (ab != null) ab.Visible = true;
                    }
                    if (ViewState["REQUISITIONSTATUS"] != null && ViewState["REQUISITIONSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 196, "CAN"))
                    {
                        if (ab != null) ab.Visible = false;
                    }
                    if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName)) ab.Visible = false;
                }
                UserControlQuick qa = ((UserControlQuick)e.Item.FindControl("ddlSealTypeAdd"));
                if (qa != null) qa.bind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}
