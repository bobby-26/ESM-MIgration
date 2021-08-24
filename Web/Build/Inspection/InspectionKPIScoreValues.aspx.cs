using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Inspection_InspectionKPIScoreValues : PhoenixBasePage
{
 

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["CATEGORYGROUPID"] = "";

                ViewState["CATEGORYCODE"] = "";
                if (Request.QueryString["CATEGORYGROUPID"] != null && Request.QueryString["CATEGORYGROUPID"].ToString() != string.Empty)
                    ViewState["CATEGORYGROUPID"] = Request.QueryString["CATEGORYGROUPID"].ToString();
            }




            //BindData();
            //SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {
        string[] alColumns = { "FLDCATEGORYID", "FLDSUBCATEGORYID" };
        string[] alCaptions = { "Category", "Sub Category" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //string docname = "";

        DataTable dt = PhoenixKPIRegisters.kpiscoringcriteriasearch(General.GetNullableGuid(ViewState["CATEGORYGROUPID"].ToString()));

        // General.SetPrintOptions("gvkpiscoringcriteria", docname, alCaptions, alColumns, ds.Tables.Add(dt));

        if (dt.Rows.Count > 0)
        {
            txteventname.Text = dt.Rows[0]["FLDCATEGORYNAME"].ToString();
            txtcategorytype.Text = dt.Rows[0]["FLDCATEGORYSHORTCODE"].ToString();
            gvkpiscoringcriteria.DataSource = dt;           
        }
        else
        {
            gvkpiscoringcriteria.DataSource = dt;
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    protected void gvkpiscoringcriteria_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();

            //SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvkpiscoringcriteria_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

       protected void gvkpiscoringcriteria_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvkpiscoringcriteria_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
          
           

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadDropDownList ddlsubcategoryadd = (RadDropDownList)e.Item.FindControl("ddlsubcategoryadd");
                UserControlMaskNumber txtmaxscoreadd = (UserControlMaskNumber)e.Item.FindControl("txtmaxscoreadd");
                UserControlMaskNumber txtminadd = (UserControlMaskNumber)e.Item.FindControl("txtminadd");
                UserControlMaskNumber txtmaxadd = (UserControlMaskNumber)e.Item.FindControl("txtmaxadd");
                UserControlMaskNumber txtscoreadd = (UserControlMaskNumber)e.Item.FindControl("txtscoreadd");

                RadDropDownList ddlmeasurecodeadd = (RadDropDownList)e.Item.FindControl("ddlmeasurecodeadd");

                PhoenixKPIRegisters.kpiscorecriteriainsert(null, General.GetNullableGuid(ViewState["CATEGORYGROUPID"].ToString())
                                                           , ddlmeasurecodeadd.SelectedValue.ToString()
                                                           , General.GetNullableDecimal(txtminadd.Text)
                                                           , General.GetNullableDecimal(txtmaxadd.Text)
                                                           , General.GetNullableDecimal(txtscoreadd.Text));



              
                BindData();
                gvkpiscoringcriteria.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadLabel lblcriteriaid = (RadLabel)e.Item.FindControl("lblcriteriaid");
                UserControlMaskNumber txtminedit = (UserControlMaskNumber)e.Item.FindControl("txtminedit");
                UserControlMaskNumber txtmaxedit = (UserControlMaskNumber)e.Item.FindControl("txtmaxedit");
                UserControlMaskNumber txtscoreedit = (UserControlMaskNumber)e.Item.FindControl("txtscoreedit");

                PhoenixKPIRegisters.kpiscorecriteriainsert(General.GetNullableGuid(lblcriteriaid.Text)
                                                           , General.GetNullableGuid(ViewState["CATEGORYGROUPID"].ToString())
                                                           , null
                                                           , General.GetNullableDecimal(txtminedit.Text)
                                                           , General.GetNullableDecimal(txtmaxedit.Text)
                                                           , General.GetNullableDecimal(txtscoreedit.Text));


             
                BindData();
                gvkpiscoringcriteria.Rebind();
            }
            if(e.CommandName.ToUpper()=="DELETE")
            {
              
                PhoenixKPIRegisters.kpiscorecriteriadelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblcriteriaid")).Text));
                BindData();
                gvkpiscoringcriteria.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvkpiscoringcriteria_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
      
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
}
