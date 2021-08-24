using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class Registers_RegisterProsperScoringCriteria : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                if(!IsPostBack)
                {
                    if (Request.QueryString["CATEGORYID"] != null && Request.QueryString["CATEGORYID"].ToString() != string.Empty)
                    {
                        ViewState["CATEGORYID"] = Request.QueryString["CATEGORYID"].ToString();

                        if (Request.QueryString["MEASUREID"] != null && Request.QueryString["MEASUREID"].ToString() != string.Empty)
                        {
                            ViewState["MEASUREID"] = Request.QueryString["MEASUREID"].ToString();
                            EditScoringCriteria();
                        }
                    }
                    ViewState["PAGENUMBER"] = 1;
                    ViewState["SORTEXPRESSION"] = null;
                    ViewState["SORTDIRECTION"] = null;
                    gvprosperscoringcriteria.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                 }
            }
            BindData();
            gvprosperscoringcriteria.Rebind();
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void EditScoringCriteria()
    {
        string categoryid = (ViewState["CATEGORYID"] == null) ? null : ViewState["CATEGORYID"].ToString();
        string measureid = (ViewState["MEASUREID"] == null) ? null : ViewState["MEASUREID"].ToString();
         
        DataTable dt = PhoenixRegisterProsperScoringCriteria.EditProsperScoringCriteria(General.GetNullableGuid(categoryid), General.GetNullableGuid(measureid));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtCategoryName.Text = dr["FLDCATEGORYNAME"].ToString();
            txtMeasureName.Text = dr["FLDMEASURENAME"].ToString();
        }
    }
   
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCATEGORYNAME", "FLDMEASURENAME" };
        string[] alCaptions = { "Category", "Measure" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        string categoryid = (ViewState["CATEGORYID"] == null) ? null : ViewState["CATEGORYID"].ToString();
        string measureid = (ViewState["MEASUREID"] == null) ? null : ViewState["MEASUREID"].ToString();

        DataSet ds = PhoenixRegisterProsperScoringCriteria.ProsperScoringCriteriaSearch(General.GetNullableGuid(categoryid)
                                                                            , General.GetNullableGuid(measureid)
                                                                            , sortexpression
                                                                            , sortdirection
                                                                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                            , gvprosperscoringcriteria.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);
        General.SetPrintOptions("gvprospermeasuremap", "Category", alCaptions, alColumns, ds);
        gvprosperscoringcriteria.DataSource = ds;
        gvprosperscoringcriteria.VirtualItemCount = iRowCount;
      

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
   

      
    private bool IsValidCategoryAndMeasure(string MinimumValue, string MaximumValue, string Score)
    {
        ucError.HeaderMessage = "Please provide the following required information";

       

        if (MinimumValue.Trim().Equals(""))
            ucError.ErrorMessage = "Minimum Range is required";

        if (MaximumValue.Trim().Equals(""))
            ucError.ErrorMessage = "Maximum Range is required.";

        if (Score.Trim().Equals(""))
            ucError.ErrorMessage = "Score is required.";

        if(Convert.ToDecimal(MaximumValue) < Convert.ToDecimal(MinimumValue))
            ucError.ErrorMessage = "Minimum range must be less than maximum range.";
        return (!ucError.IsError);
    }

    protected void gvprosperscoringcriteria_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvprosperscoringcriteria.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvprosperscoringcriteria_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
         
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
               

                PhoenixRegisterProsperScoringCriteria.InsertProsperScoringCriteria(
                    General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                    , General.GetNullableGuid(ViewState["MEASUREID"].ToString())
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtminadd")).Text.ToString())
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtmaxadd")).Text.ToString())
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtscoreadd")).Text.ToString())
                   );
             
                BindData();
                gvprosperscoringcriteria.Rebind();
            }
            if(e.CommandName.ToUpper()=="DELETE")
            {
               
                string scorecriteriaid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDSCORINGCRITERIAID"].ToString();

                PhoenixRegisterProsperScoringCriteria.DeleteProsperScoringCriteria(General.GetNullableGuid(scorecriteriaid));
               
                BindData();
                gvprosperscoringcriteria.Rebind();
            }
            if(e.CommandName.ToUpper()=="UPDATE")
            {
               
                PhoenixRegisterProsperScoringCriteria.UpdateProsperScoringCriteria(
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblscorecriteriaid")).Text.ToString())
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtminedit")).Text)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtmaxedit")).Text)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtscoreedit")).Text)
                   );
              
                BindData();
                gvprosperscoringcriteria.Rebind();

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

    protected void gvprosperscoringcriteria_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvprosperscoringcriteria_SortCommand(object sender, GridSortCommandEventArgs e)
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
