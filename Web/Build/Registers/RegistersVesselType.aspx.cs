using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersVesselType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersVesselType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('RadGrid1')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersVesselType.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        MenuVesselType.AccessRights = this.ViewState;
        MenuVesselType.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            RadGrid1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void MenuVesselType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                RadGrid1.CurrentPageIndex = 0;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if(e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)RadGrid1.MasterTableView.GetItems(GridItemType.Footer)[0];
                if (!IsValidVesselType(((RadTextBox)item.FindControl("txtVesselcodeAdd")).Text,
                   ((UserControlHard)item.FindControl("ucCategoryAdd")).SelectedHard,
                   ((RadTextBox)item.FindControl("txtVesselTypeDescriptionAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertVesselType(
                     ((RadTextBox)item.FindControl("txtVesselcodeAdd")).Text,
                     ((UserControlHard)item.FindControl("ucCategoryAdd")).SelectedHard,
                     ((RadTextBox)item.FindControl("txtVesselTypeDescriptionAdd")).Text,
                     (((RadCheckBox)item.FindControl("chkActiveYNAdd")).Checked.Equals(true)) ? 1 : 0,
                     (((RadCheckBox)item.FindControl("chkShowPlannerGridYNAdd")).Checked.Equals(true)) ? 1 : 0,
                     General.GetNullableGuid(((RadComboBox)item.FindControl("ddlEUVesselTypeAdd")).SelectedValue)
                     , (((RadCheckBox)item.FindControl("chkShowDocregYNAdd")).Checked.Equals(true)) ? 1 : 0
                );

                ucStatus.Text = "Information Added";
                Rebind();
            }
            GridEditableItem eeditedItem = e.Item as GridEditableItem;

            if (e.CommandName == "InitInsert")
            {
                RadGrid1.EditIndexes.Clear();
            }
            if (e.CommandName == RadGrid.EditCommandName)
            {
                e.Item.OwnerTableView.IsItemInserted = false;
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string VesseltypeId = ((RadLabel)e.Item.FindControl("lblVesseltypeId")).Text;
                if (VesseltypeId == string.Empty || VesseltypeId == "")
                {
                    
                }
                else
                {
                    string vesseltypecode = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDVESSELTYPEID"].ToString();

                    if (!IsValidVesselType(((RadTextBox)eeditedItem.FindControl("txtVesselcodeEdit")).Text,
                            ((UserControlHard)eeditedItem.FindControl("ucCategoryEdit")).SelectedHard,
                            ((RadTextBox)eeditedItem.FindControl("txtVesselTypeDescriptionEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    UpdateVesselType(
                         Int32.Parse(vesseltypecode),
                         ((RadTextBox)eeditedItem.FindControl("txtVesselcodeEdit")).Text,
                         ((UserControlHard)eeditedItem.FindControl("ucCategoryEdit")).SelectedHard,
                         ((RadTextBox)eeditedItem.FindControl("txtVesselTypeDescriptionEdit")).Text,
                         (((RadCheckBox)eeditedItem.FindControl("chkActiveYNEdit")).Checked.Equals(true)) ? 1 : 0,
                         (((RadCheckBox)eeditedItem.FindControl("chkShowPlannerGridYNEdit")).Checked.Equals(true)) ? 1 : 0,
                         General.GetNullableGuid(((RadComboBox)eeditedItem.FindControl("ddlEUVesselTypeEdit")).SelectedValue)
                     , (((RadCheckBox)eeditedItem.FindControl("chkShowdocregYNEdit")).Checked.Equals(true)) ? 1 : 0);
                         
                     
                    Rebind();
                }
            }
            if(e.CommandName == "Page")
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELTYPECODE", "FLDHARDNAME", "FLDTYPEDESCRIPTION", "FLDEUVESSELTYPE", "FLDACTIVE", "FLDSHOWPLANNERGRIDYNNAME" };
        string[] alCaptions = { "Code", "Vessel Type", "Vessel Sub-Type","EU Vessel Type", "Active Y/N", "Show Planner Grid Y/N" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersVesselType.VesselTypeSearch(0, txtSearch.Text, txtVesselDescription.Text, null, sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=VesselSubType.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Sub-Type</h3></td>");
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

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : RadGrid1.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDVESSELTYPECODE", "FLDHARDNAME", "FLDTYPEDESCRIPTION", "FLDEUVESSELTYPE", "FLDACTIVE", "FLDSHOWPLANNERGRIDYNNAME" };
            string[] alCaptions = { "Code", "Vessel Type", "Vessel Sub-Type", "EU Vessel Type", "Active Y/N", "Show Planner Grid Y/N" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixRegistersVesselType.VesselTypeSearch(0, txtSearch.Text, txtVesselDescription.Text, null, sortexpression, sortdirection,
                int.Parse(ViewState["PAGENUMBER"].ToString()),
                RadGrid1.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("RadGrid1", "Vessel Sub-Type", alCaptions, alColumns, ds);

            RadGrid1.DataSource = ds;
            RadGrid1.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        RadGrid1.SelectedIndexes.Clear();
        RadGrid1.EditIndexes.Clear();
        RadGrid1.DataSource = null;
        RadGrid1.Rebind();
    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ucCategoryEdit");
            RadLabel lblcategoryedit = (RadLabel)e.Item.FindControl("lblCategoryEdit");
            if (ucHard != null) ucHard.SelectedHard = lblcategoryedit.Text;

            RadComboBox ddl = (RadComboBox)e.Item.FindControl("ddlEUVesselTypeEdit");
            if (ddl != null)
            {
                DataSet ds = PhoenixRegistersEUMRVVesselTypeMapping.ListEUVesselType(0);
                ddl.DataSource = ds;
                ddl.DataTextField = "FLDVESSELTYPE";
                ddl.DataValueField = "FLDEUMRVVESSELTYPEID";
                ddl.DataBind();
                ddl.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddl.SelectedIndex = 0;

                ddl.SelectedValue = drv["FLDEUMRVVESSELTYPEID"].ToString();
            }
        }
        if(e.Item is GridFooterItem)
        {
            RadComboBox ddl = (RadComboBox)e.Item.FindControl("ddlEUVesselTypeAdd");
            if (ddl != null)
            {
                DataSet ds = PhoenixRegistersEUMRVVesselTypeMapping.ListEUVesselType(0);
                ddl.DataSource = ds;
                ddl.DataTextField = "FLDVESSELTYPE";
                ddl.DataValueField = "FLDEUMRVVESSELTYPEID";
                ddl.DataBind();
                ddl.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddl.SelectedIndex = 0;
            }
        }
    }
 
    protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            ViewState["VESSELTYPEID"] = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDVESSELTYPEID"].ToString();
            RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
            return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {

    }
    protected void RadGrid1_SortCommand(object sender, GridSortCommandEventArgs e)
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

    private void InsertVesselType(string vesselcode, string category, string VesselTypedescription, int isactive, int showplannergridyn, Guid? euvesseltype, int docreg)
    {

        PhoenixRegistersVesselType.InsertVesselType(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            vesselcode, Convert.ToInt32(category), VesselTypedescription, isactive, showplannergridyn, euvesseltype,docreg);
    }

    private void UpdateVesselType(int VesselTypeid, string vesselcode, string category, string VesselTypedescription, int isactive, int showplannergridyn, Guid? euvesseltype, int docreg)
    {

        PhoenixRegistersVesselType.UpdateVesselType(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            VesselTypeid, vesselcode, Convert.ToInt32(category), VesselTypedescription, isactive, showplannergridyn, euvesseltype, docreg);
    }

    private bool IsValidVesselType(string vesselcode, string category, string VesselTypedescription)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        Int16 result;
        if (vesselcode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (category == "" || !Int16.TryParse(category, out result))
            ucError.ErrorMessage = "Vessel Type is required";


        if (VesselTypedescription.Trim().Equals(""))
            ucError.ErrorMessage = "Vessel Sub-Type is required.";

        return (!ucError.IsError);
    }

    private void DeleteVesselType(int VesselTypecode)
    {
        PhoenixRegistersVesselType.DeleteVesselType(0, VesselTypecode);
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteVesselType(Int32.Parse(ViewState["VESSELTYPEID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
