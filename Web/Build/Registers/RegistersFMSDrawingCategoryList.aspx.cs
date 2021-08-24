using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class RegistersFMSDrawingCategoryList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersFMSDrawingCategoryList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFMSDrawing')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersFMSDrawingCategoryList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','FMS','DocumentManagement/DocumentManagementFMSDrawingAdd.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCOMPANY");
            toolbar.AddFontAwesomeButton("../Registers/RegistersFMSDrawingCategoryList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuRegistersFMSDrawing.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                gvFMSDrawing.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            string[] alColumns = { "FLDORDER", "FLDDRAWINGCATEGORY", "FLDDRAWINGNUMBER", "FLDDRAWINGNAME", };
            string[] alCaptions = { "Order", "Drawing Category", "Drawing No", "Drawing Name" };
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

            ds = PhoenixDocumentManagementFMSDrawings.FMSDrawingList(
                                                        txtdrawingNo.Text,
                                                        ddlvessel.SelectedValue,
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

    protected void RegistersFMSDrawing_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvFMSDrawing.SelectedIndexes.Clear();
                gvFMSDrawing.EditIndexes.Clear();
                gvFMSDrawing.DataSource = null;
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
                gvFMSDrawing.DataSource = null;
                gvFMSDrawing.Rebind();

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
        gvFMSDrawing.Rebind();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDDRAWINGNUMBER", "FLDDRAWINGNAME" };
            string[] alCaptions = { "Drawing No", "Drawing Name" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                
            string fileno = (txtdrawingNo.Text == null) ? "" : txtdrawingNo.Text;

            DataSet ds = PhoenixDocumentManagementFMSDrawings.FMSDrawingList(
                                                        txtdrawingNo.Text,
                                                        General.GetNullableInteger(ddlvessel.SelectedVessel),
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
                PhoenixDocumentManagementFMSDrawings.FMSDrawingDelete(new Guid(drawingid));
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
                RadLabel fileid = (RadLabel)e.Item.FindControl("lblFileNoId");
                RadLabel attach = (RadLabel)e.Item.FindControl("lblisattachment");                
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");

                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lbldrawingno");
                if (lnkEdit != null)
                {
                    lnkEdit.Visible = SessionUtil.CanAccess(this.ViewState, lnkEdit.CommandName);
                    lnkEdit.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'FMS', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFMSDrawingAdd.aspx?FileNoID=" + fileid.Text + "');return true;");
                }

                LinkButton cmdDisVessel = (LinkButton)e.Item.FindControl("cmdMapVesselType");
                if (cmdDisVessel != null)
                {
                    cmdDisVessel.Visible = SessionUtil.CanAccess(this.ViewState, cmdDisVessel.CommandName);
                    cmdDisVessel.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'FMS', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFMSDrawingVesselTypeAdd.aspx?drawingcategoryid=" + fileid.Text + "');return true;");
                }

                LinkButton cmdMapVessel = (LinkButton)e.Item.FindControl("cmdMapVessel");
                if (cmdMapVessel != null)
                {
                    cmdMapVessel.Visible = SessionUtil.CanAccess(this.ViewState, cmdMapVessel.CommandName);
                    cmdMapVessel.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'FMS', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFMSDrawingVesselsAdd.aspx?drawingcategoryid=" + fileid.Text + "');return true;");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
