using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Registers_RegisterProsperMeasureMapping : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvprospermeasuremap.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvprospermeasuremap.UniqueID, "Edit$" + r.RowIndex.ToString());
    //        }
    //    }
    //    base.Render(writer);
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperMeasureMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvprospermeasuremap')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperMeasureMapping.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Registers/RegisterProsperMeasureMapping.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            MenuRegistersProsper.AccessRights = this.ViewState;
            MenuRegistersProsper.MenuList = toolbar.Show();
            //MenuRegistersProsper.SetTrigger(pnlprospermm);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                //ViewState["CATEGORYID"] = null;
                //ViewState["MEASUREID"] = null;
                BindCategoryType();
                BindMeasureType();
                gvprospermeasuremap.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();
            string[] alColumns = { "FLDCATEGORYNAME", "FLDMEASURENAME" };
            string[] alCaptions = { "Category", "Measure" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            ds = PhoenixRegisterProsperMeasureMapping.ProsperMeasureMappingSearch(General.GetNullableGuid(ddlmeasurecode.SelectedValue)
                                                                                , General.GetNullableGuid(ddlcategorycode.SelectedValue)
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                , iRowCount
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount);
            General.ShowExcel("Measure Mapping", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersProsper_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
               
                ViewState["CATEGORYID"] = ddlcategorycode.SelectedValue;
                ViewState["MEASUREID"] = ddlmeasurecode.SelectedValue;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvprospermeasuremap.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlcategorycode.SelectedIndex = 0;
                ddlmeasurecode.SelectedIndex = 0;
               
                ViewState["CATEGORYID"] = null;
                ViewState["MEASUREID"] = null;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvprospermeasuremap.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCATEGORYNAME", "FLDMEASURENAME"};
        string[] alCaptions = { "Category", "Measure" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string categoryid = (ViewState["CATEGORYID"] == null) ? null : ViewState["CATEGORYID"].ToString();
        string measureid = (ViewState["MEASUREID"] == null) ? null : ViewState["MEASUREID"].ToString();

        DataSet ds = PhoenixRegisterProsperMeasureMapping.ProsperMeasureMappingSearch(General.GetNullableGuid(measureid)
                                                                            , General.GetNullableGuid(categoryid)
                                                                            , sortexpression
                                                                            , sortdirection
                                                                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                            , gvprospermeasuremap.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);
        General.SetPrintOptions("gvprospermeasuremap", "Measure Mapping", alCaptions, alColumns, ds);
        gvprospermeasuremap.DataSource = ds;
        gvprospermeasuremap.VirtualItemCount = iRowCount; 
    
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

  
     

    private bool IsValidCategoryAndMeasure(string category, string measure)
    {
        ucError.HeaderMessage = "Please provide the following required information";

     
        if (General.GetNullableGuid(category) == null)
            ucError.ErrorMessage = "Category is required.";

        if (General.GetNullableGuid(measure) == null)
            ucError.ErrorMessage = "Measure is required.";
        return (!ucError.IsError);
    }

    protected void BindCategoryType()
    {
        ddlcategorycode.Items.Clear();
        ddlcategorycode.DataSource = PhoenixRegisterProsperMeasureMapping.ProsperCategoryList();
        ddlcategorycode.DataTextField = "FLDCATEGORYNAME";
        ddlcategorycode.DataValueField = "FLDCATEGORYID";
        ddlcategorycode.DataBind();
        //ddlcategorycode.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    protected void BindMeasureType()
    {
        ddlmeasurecode.Items.Clear();
        ddlmeasurecode.DataSource = PhoenixRegisterProsperMeasureMapping.ProsperMeasureList();
        ddlmeasurecode.DataTextField = "FLDMEASURENAME";
        ddlmeasurecode.DataValueField = "FLDMEASUREID";
        ddlmeasurecode.DataBind();
       // ddlmeasurecode.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void gvprospermeasuremap_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvprospermeasuremap.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvprospermeasuremap_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
          
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidCategoryAndMeasure(ddlcategorycode.SelectedValue.ToString()
                     , ((RadComboBox)e.Item.FindControl("ddlmeasureadd")).SelectedValue.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegisterProsperMeasureMapping.InsertProsperMeasureMapping(
                   new Guid(ddlcategorycode.SelectedValue.ToString())
                    , new Guid(((RadComboBox)e.Item.FindControl("ddlmeasureadd")).SelectedValue));
              
                BindData();
                gvprospermeasuremap.Rebind();
            }
            if(e.CommandName.ToUpper()=="DELETE")
            {
              
                string mappingid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDMEASUREMAPPINGID"].ToString();


                PhoenixRegisterProsperMeasureMapping.DeleteProsperMeasureMapping(General.GetNullableGuid(mappingid));
              
                BindData();
                gvprospermeasuremap.Rebind();
            }
            if(e.CommandName.ToUpper()=="UPDATE")
            {
                
                if (!IsValidCategoryAndMeasure(((RadComboBox)e.Item.FindControl("ddlcategoryedit")).SelectedValue.ToString()
                         , ((RadComboBox)e.Item.FindControl("ddlmeasureedit")).SelectedValue.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegisterProsperMeasureMapping.UpdateProsperMeasure(
                    new Guid(((RadLabel)e.Item.FindControl("lblmeasuremappingidedit")).Text.ToString())
                   , new Guid(((RadComboBox)e.Item.FindControl("ddlcategoryedit")).SelectedValue)
                   , new Guid(((RadComboBox)e.Item.FindControl("ddlmeasureedit")).SelectedValue));
                
                BindData();
                gvprospermeasuremap.Rebind();
            }
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

    protected void gvprospermeasuremap_ItemDataBound(object sender, GridItemEventArgs e)
    {
       

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblcategorycode = (RadLabel)e.Item.FindControl("lblcategorycode");
            if (lblcategorycode != null && lblcategorycode.Text == "VTE")
            {
                LinkButton vm = (LinkButton)e.Item.FindControl("cmdvesselmap");
                if (vm != null)
                    vm.Visible = true;
            }
            else
            {
                LinkButton vm = (LinkButton)e.Item.FindControl("cmdvesselmap");
                if (vm != null)
                    vm.Visible = false;
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            //if (!e.Item.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.RowState.Equals(DataControlRowState.Edit))
            //{
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            //}

            RadComboBox category = (RadComboBox)e.Item.FindControl("ddlcategoryedit");
            RadLabel categoryid = (RadLabel)e.Item.FindControl("lblcategoryidedit");
            if (category != null && categoryid != null)
            {
                category.DataSource = PhoenixRegisterProsperMeasureMapping.ProsperCategoryList();
                category.DataBind();
                //category.Items.Insert(0, new ListItem("--Select--", "Dummy"));
                category.SelectedValue = categoryid.Text.ToString();
            }

            RadComboBox measure = (RadComboBox)e.Item.FindControl("ddlmeasureedit");
            RadLabel measureid = (RadLabel)e.Item.FindControl("lblmeasureidedit");
            if (category != null && measureid != null)
            {
                measure.DataSource = PhoenixRegisterProsperMeasureMapping.ProsperMeasureList();
                measure.DataBind();
                // measure.Items.Insert(0, new ListItem("--Select--", "Dummy"));
                measure.SelectedValue = measureid.Text.ToString();
            }

            LinkButton cmdMap = (LinkButton)e.Item.FindControl("cmdProsperMapping");

            if (cmdMap != null)
            {
                cmdMap.Visible = SessionUtil.CanAccess(this.ViewState, cmdMap.CommandName);
                cmdMap.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/Registers/RegisterProsperScoringCriteria.aspx?CATEGORYID=" + drv["FLDCATEGORYID"].ToString() + "&MEASUREID=" + drv["FLDMEASUREID"].ToString() + "');return false;");
            }

            LinkButton cmdVMap = (LinkButton)e.Item.FindControl("cmdvesselmap");
            if (cmdVMap != null)
                cmdVMap.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegisterProsperVesselMapping.aspx?CATEGORYID=" + drv["FLDCATEGORYID"].ToString() + "&MEASUREID=" + drv["FLDMEASUREID"].ToString() + "');return false;");

        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadComboBox ddlcategoryadd = (RadComboBox)e.Item.FindControl("ddlcategoryadd");
            ddlcategoryadd.Items.Clear();
            ddlcategoryadd.DataSource = PhoenixRegisterProsperMeasureMapping.ProsperCategoryList();
            ddlcategoryadd.DataTextField = "FLDCATEGORYNAME";
            ddlcategoryadd.DataValueField = "FLDCATEGORYID";
            ddlcategoryadd.DataBind();
            // ddlcategoryadd.Items.Insert(0, new ListItem("--Select--", "Dummy"));

            RadComboBox ddlmeasureadd = (RadComboBox)e.Item.FindControl("ddlmeasureadd");
            ddlmeasureadd.Items.Clear();
            ddlmeasureadd.DataSource = PhoenixRegisterProsperMeasureMapping.ProsperMeasureList();
            ddlmeasureadd.DataTextField = "FLDMEASURENAME";
            ddlmeasureadd.DataValueField = "FLDMEASUREID";
            ddlmeasureadd.DataBind();
            // ddlmeasureadd.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        }
    }

    protected void gvprospermeasuremap_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
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
