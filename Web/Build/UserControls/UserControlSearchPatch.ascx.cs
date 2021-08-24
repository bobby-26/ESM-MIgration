using System;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DefectTracker;


public partial class UserControls_UserControlSearchPatch : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvMulticolumn.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvMulticolumn.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        btnsearch.Attributes.Add("style", "visibility:hidden;");

        btnmulticolumn.Attributes.Add("onmouseover", this.ClientID + "DropMouseOver('" + btnmulticolumn.ClientID + "')");
        btnmulticolumn.Attributes.Add("onmouseout", this.ClientID + "DropMouseOut('" + btnmulticolumn.ClientID + "')");

        if (!IsPostBack)
        {
            panel1.Attributes.CssStyle.Add("position", "absolute");
            panel1.Width = Unit.Parse((pnlcover.Width.Value).ToString());           
        }
        else
        {
            BindData();
        }

    }
    public void btnmulticolumn_OnClick(object sender, EventArgs e)
    {
        if (!panel1.Visible)
        {
            panel1.Visible = true;
            ViewState["SEARCHBYPATCH"] = txtmuticolumn.Text;
        }
        else
            panel1.Visible = false;

        BindData();        
    }
    public bool Enabled
    {
        get
        {
            return txtmuticolumn.Enabled;
        }
        set
        {
            txtmuticolumn.Enabled = value;
            btnmulticolumn.Enabled = value;
            pnlcover.Enabled = value;
        }
    }
    public string Width
    {
        get
        {
            return panel1.Width.ToString();
        }
        set
        {
            string WidthVal = value;
            if (!string.IsNullOrEmpty(WidthVal))
            {
                if (WidthVal.Contains("%"))
                    return;
                int val = Int32.Parse(WidthVal.Replace("px", string.Empty));
                if (val > 343)
                {
                    panel1.Width = Unit.Parse(WidthVal);
                    pnlcover.Width = Unit.Parse(WidthVal);
                    txtmuticolumn.Width = Unit.Parse((pnlcover.Width.Value - 60).ToString());
                }
            }
        }
    }
    public string CssClass
    {
        set
        {
            pnlcover.CssClass = value;
        }
    }
    public string SelectedValue
    {
        get
        {
            if (!string.IsNullOrEmpty(txtmuticolumn.Text))
                return txtmultivalue.Text.ToString();
            else
                return txtmuticolumn.Text;
        }
        set
        {
            txtmultivalue.Text = value;
        }

    }
    public string Text
    {
        get
        {
            return txtmuticolumn.Text;
        }
        set
        {
            txtmuticolumn.Text = value;
        }
    }
    protected void btnsearch_click(object sender, EventArgs e)
    {
        if (txtmuticolumn != null)
            ViewState["SEARCHBYPATCH"] = txtmuticolumn.Text;
        else
            ViewState["SEARCHBYPATCH"] = txtPatchName.Text;

        ViewState["SEARCHBYCREATED"] = txtCreatedBy.Text;
        panel1.Visible = true;
        gvMulticolumn.EditIndex = -1;
        ViewState["CURRENTINDEX"] = -1;
        gvMulticolumn.SelectedIndex = -1;
        BindData();
    }

    private void BindData()
    {

        string searchbypatch = (ViewState["SEARCHBYPATCH"] == null) ? null : (ViewState["SEARCHBYPATCH"].ToString());
        string searchbycreated = (ViewState["SEARCHBYCREATED"] == null) ? null : (ViewState["SEARCHBYCREATED"].ToString());


        DataSet ds = PhoenixDefectTracker.SearchPatch(searchbypatch, searchbycreated);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvMulticolumn.DataSource = ds;
            gvMulticolumn.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvMulticolumn);
        }
    }

    protected void gvMulticolumn_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
            e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";

        }
    }

    protected void gvMulticolumn_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvMulticolumn, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvMulticolumn_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                panel1.Visible = false;
                txtmuticolumn.Text = ((LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkFileName")).Text.ToString();
                txtmultivalue.Text = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPatchdykey")).Text.ToString();
                OnTextChangedEvent(e);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMulticolumn_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
       
    }
    
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        
        gvMulticolumn.SelectedIndex = -1;
        gvMulticolumn.EditIndex = -1;

        ViewState["SEARCHBYPATCH"] = txtPatchName.Text;
        ViewState["SEARCHBYCREATED"] = txtCreatedBy.Text;

        BindData();
       
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
