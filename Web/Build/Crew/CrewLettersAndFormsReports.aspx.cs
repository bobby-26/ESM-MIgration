using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOperation;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using Telerik.Web.UI;
using SouthNests.Phoenix.Reports;

public partial class Crew_CrewLettersAndFormsReports : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        MenuLettersAndForms.MenuList = toolbarmain.Show();

        NameValueCollection nvc = Filter.CurrentJoiningPaperSelection;
        if (nvc != null)
        {
            ViewState["employeeid"] = nvc.Get("employeeid");
            ViewState["vesselid"] = nvc.Get("vesselid");
            ViewState["rankid"] = nvc.Get("rankid");
            if (nvc.Get("dateofjoining") != null)
                ViewState["dateofjoining"] = nvc.Get("dateofjoining");
            else
                ViewState["dateofjoining"] = "";

            if (nvc.Get("port") != null)
                ViewState["port"] = nvc.Get("port");
            else
                ViewState["port"] = "";

            if (nvc.Get("flightschedule") != null)
            {
                ViewState["flightschedule"] = nvc.Get("flightschedule");
                Session["flightschedule"] = nvc.Get("flightschedule");
            }
            else
            {
                ViewState["flightschedule"] = "";
                Session["flightschedule"] = "";
            }
            if (nvc.Get("agentaddress") != null)
            {
                ViewState["agentaddress"] = nvc.Get("agentaddress");
                Session["agentaddress"] = nvc.Get("agentaddress");
            }
            else
            {
                ViewState["agentaddress"] = "";
                Session["agentaddress"] = "";
            }
            if (nvc.Get("seatimeothercomments") != null)
                ViewState["seatimeothercomments"] = nvc.Get("seatimeothercomments");
            else
                ViewState["seatimeothercomments"] = "";

            if (nvc.Get("txtDceAddress") != null)
                ViewState["txtDceAddress"] = nvc.Get("txtDceAddress");
            else
                ViewState["txtDceAddress"] = "";

            if (nvc.Get("cargodetails") != null)
                ViewState["cargodetails"] = nvc.Get("cargodetails");
            else
                ViewState["cargodetails"] = "";

            if (nvc.Get("requesteddce") != null)
                ViewState["requesteddce"] = nvc.Get("requesteddce");
            else
                ViewState["requesteddce"] = "";
        }

    }
    protected void gvJoiningPapers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 218, 1, "ITM,ZTR,UCM,NKD,TDN,IML,PCB,PJB,STC");
            gvJoiningPapers.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSeparateJoining_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds2 = new DataSet();
            ds2 = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 220);
            gvSeparateJoining.DataSource = ds2;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDMSDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataTable dt = CrewDmsDocumentsAttachment.CrewDMSAttachmentsList();
            gvDMSDocuments.DataSource = dt;
            ViewState["referenceno"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindReferenceNo()
    {
        string refno = string.Empty;
        DataTable dt = PhoenixCrewReports.GenerateRefNumber(int.Parse(ViewState["employeeid"].ToString()), int.Parse(ViewState["vesselid"].ToString()), int.Parse(ViewState["rankid"].ToString()), ref refno);
        ViewState["referenceno"] = refno;
    }
    protected void MenuLettersAndForms_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("BACK"))
            Response.Redirect("../Crew/CrewLettersAndForms.aspx?empid=" + ViewState["employeeid"].ToString() + "&rnkid=" + ViewState["rankid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString(), true);
    }
    protected void gvSeparateJoining_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                string name = ((RadLabel)e.Item.FindControl("lblHardCode")).Text;
                BindReferenceNo();
                getSelectedReports();

                if (name == PhoenixCommonRegisters.GetHardCode(1, 220, "DPT"))
                {
                    String scriptpopup = String.Format(
                           "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=DEPARTURE&empid=" + ViewState["employeeid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rankid"].ToString() + "&dateofjoining=" + ViewState["dateofjoining"].ToString() + "&portid=" + ViewState["port"].ToString() + "&refno=" + ViewState["referenceno"].ToString() + "');");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);
                }

                if (name == PhoenixCommonRegisters.GetHardCode(1, 220, "MBF"))
                {

                    DataTable dt = PhoenixReportsCommon.LogoBind();
                    if (dt.Rows[0]["FLDLICENCECODE"].ToString() == "ESM")
                    {
                        if (ViewState["rankid"].ToString().Equals("1") || ViewState["rankid"].ToString().Equals("2"))
                        {
                            String scriptpopup = String.Format(
                                        "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=MASTERBRIEFING&empid=" + ViewState["employeeid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rankid"].ToString() + "&dateofjoining=" + ViewState["dateofjoining"].ToString() + "&portid=" + ViewState["port"].ToString() + "&refno=" + ViewState["referenceno"].ToString() + "');");
                            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);
                        }
                        else if (ViewState["rankid"].ToString().Equals("6") || ViewState["rankid"].ToString().Equals("7"))
                        {
                            String scriptpopup = String.Format(
                                       "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=CEBRIEFING&empid=" + ViewState["employeeid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rankid"].ToString() + "&dateofjoining=" + ViewState["dateofjoining"].ToString() + "&portid=" + ViewState["port"].ToString() + "&refno=" + ViewState["referenceno"].ToString() + "');");
                            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);
                        }
                    }
                    else

                    {
                        if (ViewState["rankid"].ToString().Equals("1") || ViewState["rankid"].ToString().Equals("2"))
                        {
                            String scriptpopup = String.Format(
                                        "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=MASTERBRIEFINGOTHER&empid=" + ViewState["employeeid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rankid"].ToString() + "&dateofjoining=" + ViewState["dateofjoining"].ToString() + "&portid=" + ViewState["port"].ToString() + "&refno=" + ViewState["referenceno"].ToString() + "');");
                            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                        }
                        else if (ViewState["rankid"].ToString().Equals("6") || ViewState["rankid"].ToString().Equals("7"))
                        {
                            String scriptpopup = String.Format(
                                       "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=CEBRIEFINGOTHER&empid=" + ViewState["employeeid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rankid"].ToString() + "&dateofjoining=" + ViewState["dateofjoining"].ToString() + "&portid=" + ViewState["port"].ToString() + "&refno=" + ViewState["referenceno"].ToString() + "');");
                            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);

                        }
                    }

                    //if (ViewState["rankid"].ToString().Equals("1") || ViewState["rankid"].ToString().Equals("2"))
                    //{
                    //    String scriptpopup = String.Format(
                    //                "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=MASTERBRIEFING&empid=" + ViewState["employeeid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rankid"].ToString() + "&dateofjoining=" + ViewState["dateofjoining"].ToString() + "&portid=" + ViewState["port"].ToString() + "&refno=" + ViewState["referenceno"].ToString() + "');");
                    //    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);
                    //}
                    //else if (ViewState["rankid"].ToString().Equals("6") || ViewState["rankid"].ToString().Equals("7"))
                    //{
                    //    String scriptpopup = String.Format(
                    //               "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=CEBRIEFING&empid=" + ViewState["employeeid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rankid"].ToString() + "&dateofjoining=" + ViewState["dateofjoining"].ToString() + "&portid=" + ViewState["port"].ToString() + "&refno=" + ViewState["referenceno"].ToString() + "');");
                    //    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);

                    //}
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void getSelectedReports()
    {
        string selitems = "";

        int i = 0;
        foreach (GridDataItem row in gvJoiningPapers.Items)
        {
            RadCheckBox cb = (RadCheckBox)row.FindControl("chkSelection");
            RadLabel lblhardcode = (RadLabel)row.FindControl("lblHardCode");

            if (cb != null && cb.Checked == true)
            {
                selitems += lblhardcode.Text;
                selitems += ",";
            }

            i++;
        }
        if (selitems.Length > 0)
        {
            selitems = selitems.Remove(selitems.Length - 1);
            ViewState["reports"] = selitems;
        }
        else
            ViewState["reports"] = "0";
    }

    protected void lnkJoiningPapers_Click(object sender, EventArgs e)
    {
        try
        {
            BindReferenceNo();
            getSelectedReports();
            if (ViewState["reports"].ToString() == "0")
            {
                ucError.ErrorMessage = "Select Atleast One Report";
                ucError.Visible = true;
                return;
            }

            String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=JOININGLETTERS&empid=" + ViewState["employeeid"].ToString() + "&vslid=" + ViewState["vesselid"].ToString() + "&rankid=" + ViewState["rankid"].ToString() + "&dateofjoining=" + ViewState["dateofjoining"].ToString() + "&portid=" + ViewState["port"].ToString() + "&reports=" + ViewState["reports"].ToString() + "&refno=" + ViewState["referenceno"].ToString() + "');");
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvJoiningPapers_PreRender(object sender, EventArgs e)
    {
        for (int rowIndex = gvJoiningPapers.Items.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridDataItem row = gvJoiningPapers.Items[rowIndex];
            GridDataItem previousRow = gvJoiningPapers.Items[rowIndex + 1];
            row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 : previousRow.Cells[4].RowSpan + 1;
            previousRow.Cells[4].Visible = false;
        }
    }
    protected void lnkDocumentClick_Click(object sender, EventArgs e)
    {
        try
        {
            GridDataItem row = (GridDataItem)((LinkButton)sender).NamingContainer;
            RadLabel lblDocumentId = (RadLabel)row.FindControl("lblDocumentId");
            RadLabel lblDocumentType = (RadLabel)row.FindControl("lblDocumentType");
            LinkButton lnkButton = (LinkButton)row.FindControl("lnkDocumentClick");
            if (lblDocumentType.Text.ToUpper() == "FRM")
            {
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "SetSourceURL('6',' ','" + lblDocumentId.Text + "')", true);
            }
            else if (lblDocumentType.Text.ToUpper() == "SEC")
            {
                String script = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewDMSDocumentSectionView.aspx?FLDSECTIONID=" + lblDocumentId.Text + "');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
