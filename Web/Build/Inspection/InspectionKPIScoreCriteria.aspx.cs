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

public partial class Inspection_InspectionKPIScoreCriteria : PhoenixBasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddLinkButton("../Inspection/InspectionKPIScoreCriteria.aspx", "Export to Excel", "icon_xls.png", "Excel");
            // toolbar.AddImageLink("javascript:CallPrint('gvkpiscoringcriteria')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionKPIScoreCriteria.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRegistersInspection.AccessRights = this.ViewState;
            MenuRegistersInspection.MenuList = toolbar.Show();
            // MenuRegistersInspection.SetTrigger(pnlkpiscoringcriteria);



            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                if (ddlsourcetype.SelectedIndex <= 0)
                    ddlsourcetype.SelectedIndex = 0;
                Bindcategory();

                gvkpiscoringcriteria.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }




            // BindData();
            //SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlsourcetype_Changed(object sender, EventArgs e)
    {
        Bindcategory();
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

        DataTable dt = PhoenixKPIRegisters.kpiscoringcriteriagroupsearch(General.GetNullableInteger(ddlsourcetype.SelectedValue.ToString()), null);




        // General.SetPrintOptions("gvkpiscoringcriteria", docname, alCaptions, alColumns, ds.Tables.Add(dt));
        gvkpiscoringcriteria.DataSource = dt;

    }

    protected void RegistersInspection_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {

                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvkpiscoringcriteria.Rebind();
                //SetPageNavigator();
            }

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                //ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    public void Bindcategory()
    {

        //if (ddlsourcetype.SelectedValue.ToString() == "1")
        //{
        //    ddlincident.Items.Clear();
        //    ddlincident.Items.Insert(0, new ListItem("--Select--", ""));
        //    ddlincident.Items.Insert(1, new ListItem("CATEGORY A", "CATA"));
        //    ddlincident.Items.Insert(2, new ListItem("CATEGORY B", "CATB"));
        //    ddlincident.Items.Insert(3, new ListItem("CATEGORY C", "CATC"));
        //}
        //else
        //{
        //    DataTable categorydt = PhoenixKPIRegisters.kpiCategoryList(General.GetNullableInteger(ddlsourcetype.SelectedValue.ToString()));
        //    if (categorydt.Rows.Count > 0)
        //    {
        //        ddlincident.Items.Clear();
        //        ddlincident.DataSource = categorydt;
        //        ddlincident.DataTextField = "FLDCATEGORYDNAME";
        //        ddlincident.DataValueField = "FLDCATEGORYID";
        //        ddlincident.DataBind();

        //    }
        //    ddlincident.Items.Insert(0, new ListItem("--Select--", ""));
        //}
    }


    protected void gvkpiscoringcriteria_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvkpiscoringcriteria_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void gvkpiscoringcriteria_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }
    protected void gvkpiscoringcriteria_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }

    protected void gvkpiscoringcriteria_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvkpiscoringcriteria_ItemDataBound1(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblcriteriaid = (RadLabel)e.Item.FindControl("lblcriteriaid");

            LinkButton cmdMap = (LinkButton)e.Item.FindControl("cmdMap");
            if (cmdMap != null)
            {
                int category = Convert.ToInt32(ddlsourcetype.SelectedValue.ToString());
                if (category == 1)
                {
                    cmdMap.Attributes.Add("style", "visible:false");
                    cmdMap.Visible = false;
                }
                else
                {
                    cmdMap.Attributes.Add("style", "visible:true");
                    cmdMap.Visible = true;
                    cmdMap.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionKPICategorygroupMapping.aspx?CATEGORYGROUPID=" + lblcriteriaid.Text + "&CATEGORYCODE=" + ddlsourcetype.SelectedValue.ToString() + "');return false;");
                }

            }
            LinkButton cmdkpiscore = (LinkButton)e.Item.FindControl("cmdkpiscore");
            if (cmdkpiscore != null)
            {
                cmdkpiscore.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionKPIScoreValues.aspx?CATEGORYGROUPID=" + lblcriteriaid.Text + "&CATEGORYCODE=" + ddlsourcetype.SelectedValue.ToString() + "');return false;");
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
            RadDropDownList ddlcategory = (RadDropDownList)e.Item.FindControl("ddlcategory");
            if (ddlsourcetype.SelectedValue.ToString().Equals("1"))
            {
                ddlcategory.Items.Clear();
                ddlcategory.Items.Insert(0, new DropDownListItem("--Select--", ""));
                ddlcategory.Items.Insert(1, new DropDownListItem("CATA", "CATA"));
                ddlcategory.Items.Insert(2, new DropDownListItem("CATB", "CATB"));
                ddlcategory.Items.Insert(3, new DropDownListItem("CATC", "CATC"));
            }
            else if (ddlsourcetype.SelectedValue.ToString().Equals("2"))
            {
                ddlcategory.Items.Clear();
                ddlcategory.Items.Insert(0, new DropDownListItem("--Select--", ""));
                ddlcategory.Items.Insert(1, new DropDownListItem("PSC-FSI", "PSC-FSI"));
            }
            else
            {
                ddlcategory.Items.Clear();
                ddlcategory.Items.Insert(0, new DropDownListItem("--Select--", ""));
                ddlcategory.Items.Insert(1, new DropDownListItem("CDI-SIRE", "CDI-SIRE"));
            }

        }
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
                
                RadDropDownList ddlcategory = (RadDropDownList)e.Item.FindControl("ddlcategory");
                int category = ddlcategory.SelectedIndex;
                string maxscore = txtmaxscoreadd.Text;

                if (!IsValidInstallation(category, maxscore))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixKPIRegisters.kpiscoringcriteriagroupinsert(null
                    , General.GetNullableString(ddlsourcetype.SelectedItem.Text)
                    , General.GetNullableInteger(ddlsourcetype.SelectedValue.ToString())
                    , General.GetNullableString(ddlcategory.SelectedValue.ToString())
                    , null
                    , General.GetNullableDecimal(txtmaxscoreadd.Text)

                    );
             
                BindData();
                gvkpiscoringcriteria.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadLabel lblcriteriaid = (RadLabel)e.Item.FindControl("lblcriteriaid");
                UserControlMaskNumber txtminedit = (UserControlMaskNumber)e.Item.FindControl("txtminedit");
                UserControlMaskNumber txtmaxedit = (UserControlMaskNumber)e.Item.FindControl("txtmaxedit");
                UserControlMaskNumber txtmaxscoreedit = (UserControlMaskNumber)e.Item.FindControl("txtmaxscoreedit");
                RadDropDownList ddlcategory = (RadDropDownList)e.Item.FindControl("ddlcategory");

                PhoenixKPIRegisters.kpiscoringcriteriagroupinsert(General.GetNullableGuid(lblcriteriaid.Text)
                    , General.GetNullableString(ddlsourcetype.SelectedItem.Text)
                    , General.GetNullableInteger(ddlsourcetype.SelectedValue.ToString())
                    , null
                    , null
                    , General.GetNullableDecimal(txtmaxscoreedit.Text)

                    );
               
                BindData();
                gvkpiscoringcriteria.Rebind();
            }
            if (e.CommandName.ToUpper() == "DELETE")
            {

                PhoenixKPIRegisters.kpiscorecriteriagroupdelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblcriteriaid")).Text));
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
    private bool IsValidInstallation(int category, string maxscore)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (category <=0)
            ucError.ErrorMessage = "Category is required.";

        if (maxscore.Trim().Equals(""))
            ucError.ErrorMessage = "Max score is required.";


        return (!ucError.IsError);
    }

}
