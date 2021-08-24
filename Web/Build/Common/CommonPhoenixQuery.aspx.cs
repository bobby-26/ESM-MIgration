using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CommonPhoenixQuery : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Common/CommonPhoenixQuery.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Common/CommonPhoenixQuery.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        MenuPhoenixQuery.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ddlPhoenixQuery.DataSource = PhoenixCommonPhoenixQuery.PhoenixQueryList(null, null);
            ddlPhoenixQuery.DataTextField = "FLDDESCRIPTION";
            ddlPhoenixQuery.DataValueField = "FLDQUERYID";
            ddlPhoenixQuery.DataBind();
        }
    }

    protected void MenuPhoenixQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            if (ViewState["queryid"] == null)
                return;

            NameValueCollection nvc = new NameValueCollection();
            Guid? queryid = General.GetNullableGuid(ViewState["queryid"].ToString()); //"3C3393DE-970A-4C1F-978D-5FCA9DB6817D"

            foreach (GridDataItem gvr in gvParameter.Items)
            {
                RadLabel lbl = (RadLabel)gvr.FindControl("lblParameter");
                RadLabel lblm = (RadLabel)gvr.FindControl("lblMandatory");
                RadLabel lblc = (RadLabel)gvr.FindControl("lblCaption");
                RadLabel lblDataType = (RadLabel)gvr.FindControl("lblDataType");

                if (lblDataType.Text == "DATETIME" && lblm.Text == "1")
                {
                    UserControlDate txt = (UserControlDate)gvr.FindControl("txtEntryDate");
                    if (lblm.Text.Equals("1") && (General.GetNullableString(txt.Text) == null))
                    {
                        ucError.ErrorMessage = lblc.Text + " is mandatory.";
                        ucError.Visible = true;
                        return;
                    }
                    nvc.Add(lbl.Text, txt.Text);
                }
                if (lblDataType.Text != "DATETIME" && lblm.Text == "1")
                {
                    RadTextBox txt = (RadTextBox)gvr.FindControl("txtEntry");
                    if (lblm.Text.Equals("1") && (General.GetNullableString(txt.Text) == null))
                    {
                        ucError.ErrorMessage = lblc.Text + " is mandatory.";
                        ucError.Visible = true;
                        return;
                    }
                    nvc.Add(lbl.Text, txt.Text);
                }
                if (lblDataType.Text == "DATETIME" && lblm.Text == "0")
                {
                    UserControlDate txt = (UserControlDate)gvr.FindControl("txtEntryDate");
                    nvc.Add(lbl.Text, txt.Text);
                }
                if (lblDataType.Text != "DATETIME" && lblm.Text == "0")
                {
                    RadTextBox txt = (RadTextBox)gvr.FindControl("txtEntry");
                    nvc.Add(lbl.Text, txt.Text);
                }
            }
            ExecuteAndDisplay(queryid, nvc);
            gvQuery.Rebind();
        }

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            if (ViewState["queryid"] == null)
                return;

            NameValueCollection nvc = new NameValueCollection();
            Guid? queryid = General.GetNullableGuid(ViewState["queryid"].ToString()); //"3C3393DE-970A-4C1F-978D-5FCA9DB6817D"

            foreach (GridDataItem gvr in gvParameter.Items)
            {
                RadLabel lbl = (RadLabel)gvr.FindControl("lblParameter");
                //RadTextBox txt = (RadTextBox)gvr.FindControl("txtEntry");
                RadLabel lblDataType = (RadLabel)gvr.FindControl("lblDataType");
                RadLabel lblm = (RadLabel)gvr.FindControl("lblMandatory");
                if (lblDataType.Text.Equals("DATETIME"))
                {
                    UserControlDate txt = (UserControlDate)gvr.FindControl("txtEntryDate");
                    nvc.Add(lbl.Text, txt.Text);
                }
                if (lblDataType.Text != "DATETIME")
                {
                    RadTextBox txt = (RadTextBox)gvr.FindControl("txtEntry");
                    nvc.Add(lbl.Text, txt.Text);
                }
            }
            ExecuteAndExport(queryid, nvc);
        }
    }

    private void ExecuteAndExport(Guid? queryid, NameValueCollection nvc)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoenixCommonPhoenixQuery.Execute(queryid, nvc, ref iRowCount, ref iTotalPageCount);
        DataTable dtParameters = PhoenixCommonPhoenixQuery.PhoenixQueryResultList(queryid);

        string captions = "";
        string columns = "";

        foreach (DataRow dr in dtParameters.Rows)
        {
            if (captions.Equals(""))
                captions = dr["FLDCAPTION"].ToString();
            else
                captions = captions + "," + dr["FLDCAPTION"].ToString();

            if (columns.Equals(""))
                columns = dr["FLDFIELD"].ToString();
            else
                columns = columns + "," + dr["FLDFIELD"].ToString();
        }

        string[] alColumns = columns.Split(',');
        string[] alCaptions = captions.Split(',');

        General.ShowExcel("Query", dt, alColumns, alCaptions, null, null);
    }

    private void ExecuteAndDisplay(Guid? queryid, NameValueCollection nvc)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataTable dt = PhoenixCommonPhoenixQuery.Execute(queryid, nvc, ref iRowCount, ref iTotalPageCount);
            ShowQuery(queryid, dt);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowParameters(Guid? queryid)
    {
        DataTable dt = PhoenixCommonPhoenixQuery.PhoenixQueryParameters(queryid);

        gvParameter.DataSource = dt;
        // gvParameter.DataBind();
    }

    private void ShowQuery(Guid? queryid, DataTable dt)
    {
        int columncount = AddTemplateColumn(queryid, dt);

        //if (columncount > 0)
        //{
        gvQuery.DataSource = dt;
        //gvQuery.DataBind();
        //}
    }
    private int AddTemplateColumn(Guid? queryid, DataTable dt)
    {
        string columnlist = "";
        foreach (DataColumn dc in dt.Columns)
        {
            columnlist += dc.ColumnName + ",";
        }

        DataTable dtResultFields = PhoenixCommonPhoenixQuery.PhoenixQueryResultFields(queryid, columnlist);

        foreach (DataRow dr in dtResultFields.Rows)
        {
            //TemplateField field = new TemplateField();
            //field.HeaderTemplate = new GridViewTemplate(ListItemType.Header, dr["FLDCAPTION"].ToString());
            //field.ItemTemplate = new GridViewTemplate(ListItemType.Item, dr["FLDFIELD"].ToString(), "RadLabel");
            //gvQuery.Columns.Add(field);
            GridBoundColumn boundColumn;
            boundColumn = new GridBoundColumn();
            boundColumn.DataField = dr["FLDFIELD"].ToString();
            boundColumn.HeaderText = dr["FLDCAPTION"].ToString();
            gvQuery.MasterTableView.Columns.Add(boundColumn);
        }

        return dt.Rows.Count;
    }

    protected void gvParameter_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (ddlPhoenixQuery.SelectedValue.ToUpper() != "DUMMY")
        {
            ViewState["queryid"] = ddlPhoenixQuery.SelectedValue;
            Guid? queryid = General.GetNullableGuid(ViewState["queryid"].ToString());
            ShowParameters(queryid);
        }
    }
    protected void RebindParameter()
    {
        gvParameter.SelectedIndexes.Clear();
        gvParameter.EditIndexes.Clear();
        gvParameter.DataSource = null;
        gvParameter.Rebind();
    }
    protected void gvParameter_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (drv["FLDDATATYPE"].ToString().ToUpper().Equals("DATETIME") && drv["FLDMANDATORYYN"].ToString().ToUpper().Equals("1"))
            {
                //AjaxControlToolkit.CalendarExtender cln = (AjaxControlToolkit.CalendarExtender)e.Item.FindControl("CalendarExtender2");
                //cln.TargetControlID = "txtEntry";
                UserControlDate date = (UserControlDate)e.Item.FindControl("txtEntryDate");
                date.Visible = true;
                date.CssClass = "input_mandatory";
                RadTextBox txt = (RadTextBox)e.Item.FindControl("txtEntry");
                txt.Visible = false;
            }

            if (drv["FLDDATATYPE"].ToString().ToUpper().Equals("DATETIME") && drv["FLDMANDATORYYN"].ToString().ToUpper().Equals("0"))
            {
                UserControlDate date = (UserControlDate)e.Item.FindControl("txtEntryDate");
                date.Visible = true;
                date.CssClass = "input";
                RadTextBox txt = (RadTextBox)e.Item.FindControl("txtEntry");
                txt.Visible = false;
            }
            if (drv["FLDDATATYPE"].ToString().ToUpper() != "DATETIME" && drv["FLDMANDATORYYN"].ToString().ToUpper().Equals("1"))
            {
                RadTextBox txt = (RadTextBox)e.Item.FindControl("txtEntry");
                txt.Visible = true;
                txt.CssClass = "input_mandatory";
                UserControlDate date = (UserControlDate)e.Item.FindControl("txtEntryDate");
                date.Visible = false;
            }
            if (drv["FLDDATATYPE"].ToString().ToUpper() != "DATETIME" && drv["FLDMANDATORYYN"].ToString().ToUpper().Equals("0"))
            {
                RadTextBox txt = (RadTextBox)e.Item.FindControl("txtEntry");
                txt.Visible = true;
                txt.CssClass = "input";
                UserControlDate date = (UserControlDate)e.Item.FindControl("txtEntryDate");
                date.Visible = false;
            }
        }
    }

    protected void gvQuery_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (ViewState["queryid"] == null)
        {

            NameValueCollection nvc = new NameValueCollection();
            Guid? queryid = General.GetNullableGuid(ViewState["queryid"].ToString()); //"3C3393DE-970A-4C1F-978D-5FCA9DB6817D"

            foreach (GridDataItem gvr in gvParameter.Items)
            {
                RadLabel lbl = (RadLabel)gvr.FindControl("lblParameter");
                RadLabel lblm = (RadLabel)gvr.FindControl("lblMandatory");
                RadLabel lblc = (RadLabel)gvr.FindControl("lblCaption");
                RadLabel lblDataType = (RadLabel)gvr.FindControl("lblDataType");

                if (lblDataType.Text == "DATETIME" && lblm.Text == "1")
                {
                    UserControlDate txt = (UserControlDate)gvr.FindControl("txtEntryDate");
                    if (lblm.Text.Equals("1") && (General.GetNullableString(txt.Text) == null))
                    {
                        ucError.ErrorMessage = lblc.Text + " is mandatory.";
                        ucError.Visible = true;
                        return;
                    }
                    nvc.Add(lbl.Text, txt.Text);
                }
                if (lblDataType.Text != "DATETIME" && lblm.Text == "1")
                {
                    RadTextBox txt = (RadTextBox)gvr.FindControl("txtEntry");
                    if (lblm.Text.Equals("1") && (General.GetNullableString(txt.Text) == null))
                    {
                        ucError.ErrorMessage = lblc.Text + " is mandatory.";
                        ucError.Visible = true;
                        return;
                    }
                    nvc.Add(lbl.Text, txt.Text);
                }
                if (lblDataType.Text == "DATETIME" && lblm.Text == "0")
                {
                    UserControlDate txt = (UserControlDate)gvr.FindControl("txtEntryDate");
                    nvc.Add(lbl.Text, txt.Text);
                }
                if (lblDataType.Text != "DATETIME" && lblm.Text == "0")
                {
                    RadTextBox txt = (RadTextBox)gvr.FindControl("txtEntry");
                    nvc.Add(lbl.Text, txt.Text);
                }
            }
            ExecuteAndDisplay(queryid, nvc);
        }
        if (ViewState["queryid"] != null)
        {
            ViewState["queryid"] = ddlPhoenixQuery.SelectedValue;
            Guid? queryid = General.GetNullableGuid(ViewState["queryid"].ToString());
            ShowParameters(queryid);
        }
    }
    protected void ddlPhoenixQuery_TextChanged(object sender, EventArgs e)
    {
        RebindParameter();
    }

    protected void ddlPhoenixQuery_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ViewState["queryid"] = ddlPhoenixQuery.SelectedValue;
        Guid? queryid = General.GetNullableGuid(ViewState["queryid"].ToString());
        //ShowParameters(queryid);
        gvParameter.DataSource = null;
        gvParameter.Rebind();
    }
}

public class GridViewTemplate : ITemplate
{
    ListItemType _templatetype;
    string _columnname;
    string _fieldtype;
    public GridViewTemplate(ListItemType type, string columnname)
    {
        _templatetype = type;
        _columnname = columnname;
        _fieldtype = "RadLabel";
    }
    public GridViewTemplate(ListItemType type, string columnname, string fieldtype)
    {
        _templatetype = type;
        _columnname = columnname;
        _fieldtype = fieldtype;
    }
    private void OnDataBinding(object sender, EventArgs e)
    {
        Control ctl = (Control)sender;

        IDataItemContainer dataitemcontainer = (IDataItemContainer)ctl.NamingContainer;
        object boundvalue = DataBinder.Eval(dataitemcontainer.DataItem, _columnname);

        switch (_templatetype)
        {
            case ListItemType.Item:
                PopulateField(sender, _fieldtype, boundvalue);
                break;

            case ListItemType.Header:
                PopulateField(sender, _fieldtype, boundvalue);
                break;
        }
    }
    private void PopulateField(object sender, string fieldtype, object value)
    {
        switch (fieldtype.ToUpper())
        {
            case "RadLabel":
                RadLabel lbl = (RadLabel)sender;
                lbl.Text = value.ToString();
                break;
            case "LINKBUTTON":
                LinkButton lnk = (LinkButton)sender;
                lnk.Text = value.ToString();
                break;
        }
    }

    void ITemplate.InstantiateIn(System.Web.UI.Control container)
    {
        switch (_templatetype)
        {
            case ListItemType.Header:
                Literal ltl = new Literal();
                ltl.Text = _columnname;
                container.Controls.Add(ltl);
                break;

            case ListItemType.Item:
                switch (_fieldtype.ToUpper())
                {
                    case "RadLabel":
                        RadLabel lbl = new RadLabel();
                        lbl.DataBinding += new EventHandler(OnDataBinding);
                        container.Controls.Add(lbl);
                        break;
                    case "LINKBUTTON":
                        LinkButton lnk = new LinkButton();
                        lnk.DataBinding += new EventHandler(OnDataBinding);
                        container.Controls.Add(lnk);
                        break;
                    default:
                        ltl = new Literal();
                        ltl.DataBinding += new EventHandler(OnDataBinding);
                        container.Controls.Add(ltl);
                        break;
                }
                break;

            case ListItemType.EditItem:
                break;

            case ListItemType.Footer:
                break;
        }
    }
}

