using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CommonPhoenixAuditTrail : PhoenixBasePage
{
    NameValueCollection nvc = new NameValueCollection();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string[] auditparameters = Request.QueryString.AllKeys;
            foreach (string s in auditparameters)
                nvc.Add(s, Request.QueryString[s]);
            nvc.Add("changes", "0");

            if (!IsPostBack)
            {
                string dtkey = Request.QueryString["dtkey"].ToString();
                string shortcode = Request.QueryString["shortcode"].ToString();

                //PhoenixToolbar toolbar = new PhoenixToolbar();
                ////toolbar.AddImageButton("../Common/CommonPhoenixAuditTrail.aspx?dtkey=" + dtkey + "&shortcode=" + shortcode, "Search", "search.png", "SEARCH");
                //toolbar.AddImageButton("../Common/CommonPhoenixAuditTrail.aspx?dtkey=" + dtkey + "&shortcode=" + shortcode, "Export to Excel", "icon_xls.png", "EXCEL");
                //MenuPhoenixQuery.MenuList = toolbar.Show();

                PhoenixToolbar toolbarAudit = new PhoenixToolbar();
                toolbarAudit.AddButton("Audit Changes", "AUDITCHANGES",ToolBarDirection.Left);
                toolbarAudit.AddButton("Audit Trail", "AUDITTRAIL",ToolBarDirection.Left);
                MenuPhoenixAudit.MenuList = toolbarAudit.Show();
                MenuPhoenixAudit.SelectedMenuIndex = 1;
                ExecuteAndDisplay(nvc, General.GetNullableGuid(dtkey), shortcode);
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }
    }

    protected void MenuPhoenixQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string dtkey = Request.QueryString["dtkey"].ToString();
        string shortcode = Request.QueryString["shortcode"].ToString();

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ExecuteAndExport(nvc, General.GetNullableGuid(dtkey), shortcode);
        }
    }

    protected void MenuPhoenixAudit_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string _querystring = "";
        foreach(string s in Request.QueryString.AllKeys)
        {
            if(_querystring.Equals(""))
                _querystring = _querystring + s + "=" + Request.QueryString[s];
            else
                _querystring = _querystring + "&" + s + "=" + Request.QueryString[s];
        }

        if (CommandName.ToUpper().Equals("AUDITCHANGES"))
        {
            Response.Redirect("../Common/CommonPhoenixAuditChanges.aspx?" + _querystring, false);
        }
    }

    private void ExecuteAndExport(NameValueCollection nvc, Guid? dtkey, string shortcode)
    {
        DataTable dt = PhoenixCommonAuditTrail.Execute(nvc, dtkey, shortcode);
        DataTable dtParameters = PhoenixCommonAuditTrail.PhoenixAuditResultList(dtkey, shortcode);

        string captions = "";
        string columns = "";

        foreach (DataRow dr in dtParameters.Rows)
        {
            if(captions.Equals(""))
                captions = dr["FLDCAPTION"].ToString();
            else
                captions = captions + "," + dr["FLDCAPTION"].ToString();

            if(columns.Equals(""))
                columns = dr["FLDFIELD"].ToString();
            else
                columns = columns + "," + dr["FLDFIELD"].ToString();
        }

        string[] alColumns = columns.Split(',');
        string[] alCaptions = captions.Split(',');

        General.ShowExcel("Query", dt, alColumns, alCaptions, null, null);
    }

    private void ExecuteAndDisplay(NameValueCollection nvc, Guid? dtkey, string shortcode)
    {
        DataTable dt = PhoenixCommonAuditTrail.Execute(nvc, dtkey, shortcode);
        ShowQuery(shortcode, dt);
    }

    private void ShowQuery(string shortcode, DataTable dt)
    {
        //int columncount = AddTemplateColumn(shortcode, dt);
        string columnlist = "";
        foreach (DataColumn dc in dt.Columns)
        {
            columnlist += dc.ColumnName + ",";
        }

        DataTable dtResultFields = PhoenixCommonAuditTrail.PhoenixAuditResultFields(shortcode, columnlist);

        foreach (DataRow dr in dtResultFields.Rows)
        {
            GridBoundColumn boundColumn;
            boundColumn = new GridBoundColumn();
            boundColumn.DataField = dr["FLDFIELD"].ToString();
            boundColumn.HeaderText = dr["FLDCAPTION"].ToString();
            boundColumn.UniqueName = dr["FLDFIELD"].ToString();
            gvAuditTrail.MasterTableView.Columns.Add(boundColumn);

            //gvAuditTrail.Columns.Add(boundColumn);
        }
        
        if (dt.Rows.Count > 0)
        {
            gvAuditTrail.DataSource = dt;
        }
    }

    //private int AddTemplateColumn(string shortcode, DataTable dt)
    //{
    //    string columnlist = "";
    //    foreach (DataColumn dc in dt.Columns)
    //    {
    //        columnlist += dc.ColumnName + ",";
    //    }

    //    DataTable dtResultFields = PhoenixCommonAuditTrail.PhoenixAuditResultFields(shortcode, columnlist);

    //    foreach (DataRow dr in dtResultFields.Rows)
    //    {
    //        GridTemplateColumn field = new GridTemplateColumn();
    //        field.HeaderTemplate = new GridViewTemplate(ListItemType.Header, dr["FLDCAPTION"].ToString());
    //        field.ItemTemplate = new GridViewTemplate(ListItemType.Item, dr["FLDFIELD"].ToString(), "Label");
    //        gvAuditTrail.Columns.Add(field);
    //    }

    //    return dt.Rows.Count;

    //}

    protected void gvAuditTrail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

    }
}

//public class GridViewTemplate : ITemplate
//{
//    ListItemType _templatetype;
//    string _columnname;
//    string _fieldtype;


//    public GridViewTemplate(ListItemType type, string columnname)
//    {
//        _templatetype = type;
//        _columnname = columnname;
//        _fieldtype = "Label";
//    }

//    public GridViewTemplate(ListItemType type, string columnname, string fieldtype)
//    {
//        _templatetype = type;
//        _columnname = columnname;
//        _fieldtype = fieldtype;
//    }

//    private void OnDataBinding(object sender, EventArgs e)
//    {
//        Control ctl = (Control)sender;

//        IDataItemContainer dataitemcontainer = (IDataItemContainer)ctl.NamingContainer;
//        object boundvalue = DataBinder.Eval(dataitemcontainer.DataItem, _columnname);

//        switch (_templatetype)
//        {
//            case ListItemType.Item:
//                PopulateField(sender, _fieldtype, boundvalue);
//                break;

//            case ListItemType.Header:
//                PopulateField(sender, _fieldtype, boundvalue);
//                break;
//        }
//    }

//    private void PopulateField(object sender, string fieldtype, object value)
//    {
//        switch (fieldtype.ToUpper())
//        {
//            case "LABEL":
//                Label lbl = (Label)sender;
//                lbl.Text = value.ToString();
//                break;
//            case "LINKBUTTON":
//                LinkButton lnk = (LinkButton)sender;
//                lnk.Text = value.ToString();
//                break;
//        }
//    }

//    void ITemplate.InstantiateIn(System.Web.UI.Control container)
//    {
//        switch (_templatetype)
//        {
//            case ListItemType.Header:
//                Literal ltl = new Literal();
//                ltl.Text = _columnname;
//                container.Controls.Add(ltl);
//                break;

//            case ListItemType.Item:
//                switch (_fieldtype.ToUpper())
//                {
//                    case "LABEL":
//                        Label lbl = new Label();
//                        lbl.DataBinding += new EventHandler(OnDataBinding);
//                        container.Controls.Add(lbl);
//                        break;
//                    case "LINKBUTTON":
//                        LinkButton lnk = new LinkButton();
//                        lnk.DataBinding += new EventHandler(OnDataBinding);
//                        container.Controls.Add(lnk);
//                        break;
//                    default:
//                        ltl = new Literal();
//                        ltl.DataBinding += new EventHandler(OnDataBinding);
//                        container.Controls.Add(ltl);
//                        break;
//                }
//                break;

//            case ListItemType.EditItem:
//                break;

//            case ListItemType.Footer:
//                break;
//        }
//    }
//}
