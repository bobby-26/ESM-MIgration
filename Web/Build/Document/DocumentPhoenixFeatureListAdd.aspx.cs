using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Document;

public partial class DocumentPhoenixFeatureListAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (Request.QueryString["MENUCODE"] != null && Request.QueryString["MENUCODE"].ToString() != string.Empty)
                ViewState["MENUCODE"] = Request.QueryString["MENUCODE"].ToString();
            else
                ViewState["MENUCODE"] = null;
            
            BindPath();
        }
        BindFeatureList();
    }
    protected void BindPath()
    {
        DataSet ds = new DataSet();
        ds = PhoenixDocumentFeatureList.MenuPath(int.Parse (ViewState["MENUCODE"].ToString ()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            MenuPath.Text = dr["FLDPATH"].ToString();
        }
       
    }

    protected void BindFeatureList()
    {
        DataSet ds = new DataSet();

        ds = PhoenixDocumentFeatureList.FeatureList(int.Parse(ViewState["MENUCODE"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvMenu.DataSource = ds;
            gvMenu.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvMenu);
        }

    }
    protected void gvMenu_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            if (!IsValidFeature(((TextBox)_gridview.FooterRow.FindControl("txtFeatureAdd")).Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixDocumentFeatureList.FeatureListAdd(int.Parse(ViewState["MENUCODE"].ToString()),
                                ((TextBox)_gridview.FooterRow.FindControl("txtFeatureAdd")).Text);
        }
        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidFeature(((TextBox)_gridview.Rows[nCurrentRow].FindControl("txtFeatureEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixDocumentFeatureList.FeatureListUpdate(new Guid (((Label)_gridview.Rows[nCurrentRow].FindControl("lblFeatureIDEdit")).Text.ToString()),
                                ((TextBox)_gridview.Rows[nCurrentRow].FindControl("txtFeatureEdit")).Text);
        }
        _gridview.EditIndex = -1;
        //BindFeatureList();
        BindFeatureList();

    }
    protected void gvMenu_RowUpdating(object sender, GridViewUpdateEventArgs de)
    {
        GridView _gridview = (GridView)sender;
        int nCurrentRow = de.RowIndex;
        
        if (!IsValidFeature(((TextBox)_gridview.Rows[nCurrentRow].FindControl("txtFeatureEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixDocumentFeatureList.FeatureListUpdate(new Guid (((Label)_gridview.Rows[nCurrentRow].FindControl("lblFeatureIDEdit")).Text.ToString()),
                                ((TextBox)_gridview.Rows[nCurrentRow].FindControl("txtFeatureEdit")).Text);

            _gridview.EditIndex = -1;
            BindFeatureList();
     }

    protected void gvMenu_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindFeatureList();
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }

    protected void gvMenu_RowCancelingEditing(object sender, GridViewCancelEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindFeatureList();
    }
    protected void gvMenu_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label menucode = (Label)e.Row.FindControl("lblMenuCode");
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");

            if (menucode != null)
            {
                if (eb != null)
                    eb.Attributes.Add("onclick", "javascript:parent.Openpopup('code1','','../Document/DocumentPhoenixFeatureListAdd.aspx?MENUCODE=" + menucode.Text + "'); return true;");
            }
        }
    }
    private bool IsValidFeature(string feature)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableString(feature) == null)
            ucError.ErrorMessage = "Feature is required.";

        return (!ucError.IsError);
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
    }
}
