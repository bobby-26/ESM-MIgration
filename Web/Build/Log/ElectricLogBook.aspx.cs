using System;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using System.Text;

public partial class Log_ElectricLogBook : PhoenixBasePage
{
    int usercode;
    int vesselid;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        ShowToolBar();
        if (IsPostBack == false)
        {
            BindLogBook();
        }
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton(string.Format("javascript:openNewWindow('tankAdd','','{0}/Log/ElectricLogTankAdd.aspx'); return false;", Session["sitepath"]), "Select Tank", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        toolbarmain.AddFontAwesomeButton(Session["sitepath"] + "/Log/ElectricLogBook.aspx", "Download pdf", "<i class=\"fas fa-file-excel\"></i>", "PDF");

        gvTabStrip.AccessRights = this.ViewState;
        gvTabStrip.MenuList = toolbarmain.Show();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvTankDetails.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("PDF"))
            {
                //PdfGenerator();
                gvTankDetails.MasterTableView.ExportToPdf();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTankDetails_needdatasource(object sender, EventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTankDetails.CurrentPageIndex + 1;
        DataSet ds = PhoenixElog.ListLocation(usercode, vesselid, (int)ViewState["PAGENUMBER"], 100, ref iRowCount, ref iTotalPageCount);
        gvTankDetails.DataSource = ds;
        
    }

    private void BindLogBook()
    {
        try
        {
        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTankDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem && e.Item.IsInEditMode == false)
            {
                LinkButton editBtn = (LinkButton)e.Item.FindControl("cmdEdit");
                if (editBtn != null) editBtn.Visible = SessionUtil.CanAccess(this.ViewState, editBtn.CommandName);
                RadLabel LocationId = (RadLabel)e.Item.FindControl("lblLocationId");
                if (LocationId != null)
                {
                    editBtn.Attributes.Add("onclick", "javascript:openNewWindow('tankEdit','','" + Session["sitepath"] + "/LOG/ElectricLogTankEdit.aspx?LocationId=" + LocationId.Text + "'); return false;");
                }

                LinkButton robBtn = (LinkButton)e.Item.FindControl("cmdInitializeROB");
                RadLabel rob = (RadLabel)e.Item.FindControl("lblRob");

                if (robBtn != null) robBtn.Visible = SessionUtil.CanAccess(this.ViewState, editBtn.CommandName);
                if (robBtn != null && LocationId != null)
                {
                    string url = String.Format("javascript:openNewWindow('tankROB','','{0}/LOG/ElectricLogInitializeROB.aspx?LocationId={1}&rob={2}'); return false;", Session["sitepath"], LocationId.Text, rob.Text);
                    robBtn.Attributes.Add("onclick", url);
                }

                //LinkButton txnBtn = (LinkButton)e.Item.FindControl("cmdTranscation");
                //if (txnBtn != null)
                //{
                //    if (!SessionUtil.CanAccess(this.ViewState, txnBtn.CommandName)) txnBtn.Visible = false;
                //}
            }
            if (e.Item is GridEditableItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                LinkButton eb = (LinkButton)e.Item.FindControl("cmdInitializeROB");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTankDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblLocation = (RadLabel)e.Item.FindControl("lblLocationId");
                LinkButton db1 = (LinkButton)e.Item.FindControl("cmdDelete");
                Guid locationId = new Guid(lblLocation.Text);
                //PhoenixElog.LocationDelete(usercode, locationId);
                //PhoenixElog.DeleteItembyTank(usercode, locationId);
                gvTankDetails.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("TRANSCATION"))
            {
                Response.Redirect("../Log/ElectronicLogOperationReport.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void tvwBook_NodeClickEvent(object sender, EventArgs args)
    {
        try
        {
            RadTreeNodeEventArgs e = (RadTreeNodeEventArgs)args;
            if (e.Node.Value.ToLower() != "_root")
            {
                string selectednode = e.Node.Value.ToString();
                string selectedvalue = e.Node.Text.ToString();
                string script = string.Empty;
                if (selectedvalue == "C-12.2a Transfer from sludge tank to bilge tank")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogSludgeTransfer.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "C-11 Weekly Retention of Oil Residues")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogWeeklyEntries.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "C-12.4a Evaporation")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogEvaporationFromSludge.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "C-12.3a Incineration of oil residues by waster oil incinerator")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogIncineration.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "C-11 Manual Collection of Oil Residues")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogOilSludgeToIOPP.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "C-12.1a Disposal to oil residue via shore connection")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogShoreSludgeDisposal.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "C-12.4b Transfer of sludge to slop tank")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogSludgeFromERToDeck.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "C-12.4c Drainage of water from waste oil settling/service to  waster oil tank/drain tank")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogWaterDrainedFromSludge.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "C-12.3c Incineration of oil residues by the auxiliary boiler")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogSludgeBurningInBoiler.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }

                // code d
                else if (selectedvalue == "Bilge Well")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogBilgeWell.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "Internal Bilge")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogInternalBilge.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "OWS Operation")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogOWSOperation.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "Bilge to Sludge")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogBilgetoSludge.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "Bilge water from ER to deck/cargo Slop Tank")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogBilgewaterERToDeck.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                // code f
                else if (selectedvalue == "Feature of Oil Filtering Equiment, Oil Content Meter or Stopping Device")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogOilFilteringEquiment.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "Rectification of OWS, OCM or Stopping Device")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogRectificationOWSOCM.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                //code g
                else if (selectedvalue == "Accidental Discharge or Other exceptional discharges of Oil")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogAccidentalDischarge.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                //code h
                else if (selectedvalue == "Bunkering Fuel")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogBunkeringFuel.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "Bunkering Lube")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogBunkeringLube.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                // code I
                else if (selectedvalue == "Cargo Bilge holding tank to Tank 3.3 (E.R Bilge")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogCargoBilgeHoldingTank.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "Missed Operational Entry")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogMissedOperationalEntry.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "De-bunkering of Fuel Oil")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogDebunkeringFuelOil.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "Sealing of Overboard Valve")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogSealingOverboardValve.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }
                else if (selectedvalue == "Un-Sealing of Overboard Valve")
                {
                    script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogUnSealingOverboardValve.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }

                else
                {
                    // script = String.Format("javascript:openNewWindow('NAFA','','{0}/Log/ElectricLogSludgeTransfer.aspx?ReportCode={1}&TxnId={2}');", Session["sitepath"], "", "");
                }

                ScriptManager.RegisterStartupScript(this, typeof(Page), "openWindow", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}