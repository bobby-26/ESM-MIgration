using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class DocumentManagement_DocumentManagementFMSManualList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            // BindMenu();

            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementFMSManualList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFMSManual')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementFMSManualList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            //toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','FMS','DocumentManagement/DocumentManagementFMSManualAdd.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCOMPANY");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementFMSManualList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuRegistersFMSManual.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    ddlvessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                    ddlvessel.Enabled = false;
                }
                else
                {
                    ddlvessel.SelectedValue = 0;
                }

                ddlmanuals.DataSource = PhoenixRegisterFMSManual.FMSManualCategoryList();
                ddlmanuals.DataTextField = "FLDMANUALCATEGORY";
                ddlmanuals.DataValueField = "FLDFMSMANUALCATEGORY";
                ddlmanuals.DataBind();

                gvFMSManual.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
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

            MenuFMSManual.AccessRights = this.ViewState;
            MenuFMSManual.MenuList = toolbarm.Show();

            MenuFMSManual.SelectedMenuIndex = 6;


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

            MenuFMSManual.AccessRights = this.ViewState;
            MenuFMSManual.MenuList = toolbarmain.Show();

            MenuFMSManual.SelectedMenuIndex = 3;
        }
    }

    protected void MenuFMSManual_TabStripCommand(object sender, EventArgs e)
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
                Response.Redirect("../DocumentManagement/DocumentManagementFMSManualList.aspx?", false);
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
    protected void gvFMSManual_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
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
            string[] alColumns = { "FLDMANUALNUMBER", "FLDMANUALNAME" };
            string[] alCaptions = { "Manual No", "Manual Name" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            if ((ddlvessel.SelectedVessel != "") && (ddlmanuals.SelectedValue != ""))
            {
                ds = PhoenixRegisterFMSManual.FMSManualCategory(new Guid(ddlmanuals.SelectedValue), int.Parse(ddlvessel.SelectedVessel), sortexpression, sortdirection,
                                                                        (int)ViewState["PAGENUMBER"],
                                                                        gvFMSManual.PageSize,
                                                                        ref iRowCount,
                                                                        ref iTotalPageCount);
            }
            Response.AddHeader("Content-Disposition", "attachment; filename=FMS.xls");
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

    protected void MenuRegistersFMSManual_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                

                gvFMSManual.SelectedIndexes.Clear();
                gvFMSManual.EditIndexes.Clear();
                gvFMSManual.DataSource = null;
                gvFMSManual.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlmanuals.SelectedValue = "";
                ddlvessel.SelectedVessel = null;
                gvFMSManual.SelectedIndexes.Clear();
                gvFMSManual.EditIndexes.Clear();
                gvFMSManual.DataSource = null;
                gvFMSManual.Rebind();
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
        gvFMSManual.Rebind();
    }

    private void BindData()
    {
        try
        {

            int vesselid = 0;
            if (ddlvessel.SelectedVessel != "Dummy")
            {
                vesselid = int.Parse(ddlvessel.SelectedVessel);
            }
            else
            {
                ddlvessel.SelectedVessel = "0";
                ViewState["VESSELID"] = "0";
            }

            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDMANUALNUMBER", "FLDMANUALNAME" };
            string[] alCaptions = { "Manual No", "Manual Name" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            //if ((ddlvessel.SelectedVessel != "") && (ddlmanuals.SelectedValue != ""))
            //{
                //DataSet ds = PhoenixDocumentManagementFMSManual.FMSManualSearch(
                //                                            txtMannualNo.Text,
                //                                            General.GetNullableInteger(ddlvessel.SelectedVessel),
                //                                            sortexpression,
                //                                            sortdirection,
                //                                            gvFMSManual.CurrentPageIndex + 1,
                //                                            int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount.ToString()),
                //                                            ref iRowCount,
                //                                            ref iTotalPageCount
                //                                            );

                DataSet ds = PhoenixRegisterFMSManual.FMSManualCategory(General.GetNullableGuid(ddlmanuals.SelectedValue), int.Parse(ddlvessel.SelectedVessel), sortexpression,                                                               sortdirection,
                                                                    (int)ViewState["PAGENUMBER"],
                                                                    gvFMSManual.PageSize,
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount);

                General.SetPrintOptions("gvFMSManual", "Manual List", alCaptions, alColumns, ds);

                gvFMSManual.DataSource = ds;
                gvFMSManual.VirtualItemCount = iRowCount;
            //}
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucVessel_changed(object sender, EventArgs e)
    {
        gvFMSManual.SelectedIndexes.Clear();
        gvFMSManual.EditIndexes.Clear();
        gvFMSManual.DataSource = null;
        gvFMSManual.Rebind();
    }

    protected void gvFMSManual_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridGroupHeaderItem)
            {
                GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
                DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
                item.DataCell.Text = groupDataRow["manual"].ToString();
            }

            if (e.Item is GridEditableItem)
            {
                RadLabel fileid = (RadLabel)e.Item.FindControl("lblFileNoId");
                RadLabel attach = (RadLabel)e.Item.FindControl("lblisattachment");

                LinkButton cmdattachment = (LinkButton)e.Item.FindControl("lblManualname");
                if (cmdattachment != null)
                {
                    cmdattachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdattachment.CommandName);
                    cmdattachment.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('upload','','{0}/DocumentManagement/DocumentFMSManualAttachmentList.aspx?vesselid=" + ddlvessel.SelectedVessel + "&manualsid=" + fileid.Text + " ');", Session["sitepath"]));
                }

                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)e.Item.DataItem;

                ImageButton cmdSvyAtt = item.FindControl("cmdSvyAtt") as ImageButton;
                if (cmdSvyAtt != null)
                {
                    if (drv["FLDDTKEY"].ToString() == string.Empty) cmdSvyAtt.Attributes.Add("style", "display:none");
                    if (drv["FLDDTKEY"].ToString() == "")
                    {
                        cmdSvyAtt.ImageUrl = Session["images"] + "/no-attachment.png";
                    }
                    else
                    {
                        cmdSvyAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.DOCUMENTMANAGEMENT + "&type=FMSMANUALS&VESSELID=" + ddlvessel.SelectedVessel + "'); return false;");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFMSManual_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string uid = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDFMSMANUALID"].ToString();
                PhoenixDocumentManagementFMSManual.FMSManualDelete(new Guid(uid));
                gvFMSManual.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlmanuals_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
}