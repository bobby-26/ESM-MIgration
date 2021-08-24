using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class DocumentManagementFMSDrawingList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            BindMenu();

            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["VESSELID"] = "";
                ddlvessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                gvFMSDrawing.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    ddlvessel.Enabled = false;

                }
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementFMSDrawingList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFMSDrawing')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementFMSDrawingList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");

            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','FMS','DocumentManagement/DocumentManagementFMSDrawingAdd.aspx?vesselid=" + ddlvessel.SelectedVessel + "')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCOMPANY");

            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementFMSDrawingList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuRegistersFMSDrawing.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindMenu()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            PhoenixToolbar toolbarm = new PhoenixToolbar();

            toolbarm.AddButton("E Mails", "ESMA", ToolBarDirection.Left);
            toolbarm.AddButton("ESM Filing", "ESMF", ToolBarDirection.Left);
            toolbarm.AddButton("Shipboard Forms", "SPFF", ToolBarDirection.Left);
            toolbarm.AddButton("Office Forms", "OFFF", ToolBarDirection.Left);
            toolbarm.AddButton("Maintenance Forms", "MCFS", ToolBarDirection.Left);
            toolbarm.AddButton("Plans and Drawings", "DRWS", ToolBarDirection.Left);
            toolbarm.AddButton("Manuals", "MNSF", ToolBarDirection.Left);
            toolbarm.AddButton("Equipment Manuals", "EQSF", ToolBarDirection.Left);

            MenuFMS.AccessRights = this.ViewState;
            MenuFMS.MenuList = toolbarm.Show();

            MenuFMS.SelectedMenuIndex = 5;
        }
        else
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Shipboard Forms", "SPFF", ToolBarDirection.Left);
            toolbarmain.AddButton("Maintenance Forms", "MCFS", ToolBarDirection.Left);
            toolbarmain.AddButton("Plans and Drawings", "DRWS", ToolBarDirection.Left);
            toolbarmain.AddButton("Manuals", "MNSF", ToolBarDirection.Left);
            toolbarmain.AddButton("Equipment Manuals", "EQSF", ToolBarDirection.Left);

            MenuFMS.AccessRights = this.ViewState;
            MenuFMS.MenuList = toolbarmain.Show();
            MenuFMS.SelectedMenuIndex = 2;

            ddlvessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            ddlvessel.Enabled = false;
        }
    }

    protected void MenuFMS_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ESMA"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSMailList.aspx", false);
            }
            if (CommandName.ToUpper().Equals("ESMF"))
            {
                Response.Redirect("../DocumentManagement/DocumentFMSFileNoList.aspx", false);
            }
            if (CommandName.ToUpper().Equals("SPFF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSShipboardFormList.aspx?CATEGORYNO=2", false);
            }
            if (CommandName.ToUpper().Equals("OFFF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSOfficeFormList.aspx?CATEGORYNO=16&Callfrom=1", false);
            }
            if (CommandName.ToUpper().Equals("MCFS"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSMaintenanceHistoryTemplate.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("DRWS"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSDrawingList.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("MNSF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSVesselSurveyScheduleList.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("EQSF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSEquipmentManuals.aspx?", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFMSDrawing_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
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

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDSUBCATEGORYCODE", "FLDSUBCATEGORYNAME" };
            string[] alCaptions = { "Drawing No", "Drawing Name" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string fileno = (txtdrawingNo.Text == null) ? "" : txtdrawingNo.Text;

            ds = PhoenixDocumentManagementFMSDrawings.FMSDrawingSearch(
                                                        txtdrawingNo.Text,
                                                        int.Parse(ddlvessel.SelectedVessel),
                                                        sortexpression,
                                                        sortdirection,
                                                        gvFMSDrawing.CurrentPageIndex + 1,
                                                        int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount.ToString()),
                                                        ref iRowCount,
                                                        ref iTotalPageCount
                                                        );

            Response.AddHeader("Content-Disposition", "attachment; filename=FMSDrawing.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Company</h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucVessel_changed(object sender, EventArgs e)
    {
        gvFMSDrawing.SelectedIndexes.Clear();
        gvFMSDrawing.EditIndexes.Clear();
        gvFMSDrawing.DataSource = null;
        gvFMSDrawing.Rebind();
    }

    protected void RegistersFMSDrawing_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvFMSDrawing.CurrentPageIndex = 0;
                gvFMSDrawing.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtdrawingNo.Text = "";
                gvFMSDrawing.SelectedIndexes.Clear();
                gvFMSDrawing.EditIndexes.Clear();
                gvFMSDrawing.Rebind();

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
        //gvFMSDrawing.SelectedIndexes.Clear();
        //gvFMSDrawing.EditIndexes.Clear();
        //gvFMSDrawing.DataSource = null;
        gvFMSDrawing.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDSUBCATEGORYCODE", "FLDSUBCATEGORYNAME" };
            string[] alCaptions = { "Drawing No", "Drawing Name" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string fileno = (txtdrawingNo.Text == null) ? "" : txtdrawingNo.Text;

            if (ddlvessel.SelectedVessel != "Dummy")
            {
                DataSet ds = PhoenixDocumentManagementFMSDrawings.FMSDrawingSearch(
                                                            txtdrawingNo.Text,
                                                            int.Parse(ddlvessel.SelectedVessel),
                                                            sortexpression,
                                                            sortdirection,
                                                            gvFMSDrawing.CurrentPageIndex + 1,
                                                            int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount.ToString()),
                                                            ref iRowCount,
                                                            ref iTotalPageCount
                                                            );
                General.SetPrintOptions("gvFMSDrawing", "Drawing List", alCaptions, alColumns, ds);

                gvFMSDrawing.DataSource = ds;
                gvFMSDrawing.VirtualItemCount = iRowCount;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFMSDrawing_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                //GridEditableItem eeditedItem = e.Item as GridEditableItem;
                //string uid = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDFMSDRAWINGID"].ToString();
                string drawingid = (((RadLabel)e.Item.FindControl("lblFileNoId")).Text);
                string vesselid = (((RadLabel)e.Item.FindControl("lblvesselid")).Text);

                PhoenixDocumentManagementFMSDrawings.FMSDrawingSubCategoryDelete(new Guid(drawingid), int.Parse(vesselid));
                gvFMSDrawing.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFMSDrawing_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridGroupHeaderItem)
            {
                GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
                DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
                item.DataCell.Text = groupDataRow["Category"].ToString();
            }

            if (e.Item is GridEditableItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel fileid = (RadLabel)e.Item.FindControl("lblFileNoId");
                RadLabel attach = (RadLabel)e.Item.FindControl("lblisattachment");
                RadLabel order = (RadLabel)e.Item.FindControl("lblorder");

                LinkButton cmdedit = (LinkButton)e.Item.FindControl("lblDrawingname");
                LinkButton cmddel = (LinkButton)e.Item.FindControl("cmdDelete");
                LinkButton cmdattachment = (LinkButton)e.Item.FindControl("cmdattachment");
                DataRowView dr = (DataRowView)e.Item.DataItem;
                if (cmdedit != null)
                {
                    cmdedit.Visible = SessionUtil.CanAccess(this.ViewState, cmdedit.CommandName);

                    cmdedit.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('upload','','{0}/DocumentManagement/DocumentManagementFMSDrawingEdit.aspx?vesselid=" + ddlvessel.SelectedVessel + "&FILENOID=" + fileid.Text + " ');", Session["sitepath"]));
                    HtmlGenericControl html = new HtmlGenericControl();

                    if (dr["FLDATTACHMENTYN"].ToString() == "0")
                    {
                        html.InnerHtml = "<span class=\"icon\"><i class=\"fa-paperclip-na\"></i></span>";
                        cmdattachment.Controls.Add(html);
                    }
                    else
                    {
                        html.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-paperclip\"></i></span>";
                        cmdattachment.Controls.Add(html);
                    }

                    //cmdattachment.Attributes["onclick"] = "javascript:parent.openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + fileid.Text + "&mod="
                    //                                        + PhoenixModule.DOCUMENTMANAGEMENT + "&type=FMSDrawing" + "&BYVESSEL=true&cmdname=FMSDrawingUPLOAD&VESSELID=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "'); return false;";
                    //cmdattachment.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('upload','','{0}/DocumentManagement/DocumentFMSDrawingAttachmentList.aspx?vesselid=" + ddlvessel.SelectedVessel + "&drawingid= " + fileid.Text + " ');", Session["sitepath"]));

                    cmdattachment.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('upload','','{0}/DocumentManagement/DocumentFMSDrawingAttachmentList.aspx?vesselid=" + ddlvessel.SelectedVessel + "&drawingid=" + fileid.Text + " ');", Session["sitepath"]));

                }

                if (order.Text == "0")
                {
                    cmddel.Visible = false;
                }

                if (!e.Item.IsInEditMode)
                {
                    LinkButton db = (LinkButton)item.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmTelerik(event); return false;");
                    }
                }
                               
                LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
                if (cmdDelete != null && !SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName))
                    cmdDelete.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
