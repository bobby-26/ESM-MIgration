using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;

public partial class UserControlMultiColumnDMRVoyagePort : System.Web.UI.UserControl
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
        txtmultiportvalue.Attributes.Add("style", "visibility:hidden;");
        txtmultiportcallvalue.Attributes.Add("style", "visibility:hidden;");
        btnsearch.Attributes.Add("style", "visibility:hidden;");

        if (!IsPostBack)
        {
            panel1.Attributes.CssStyle.Add("position", "absolute");
            panel1.Width = Unit.Parse((txtmuticolumn.Width.Value + btnmulticolumn.Width.Value).ToString());
        }
        else
        {
            if (panel1.Visible)
            {
                BindData();
            }
        }
    }

    public void btnmulticolumn_OnClick(object sender, EventArgs e)
    {
        if (!panel1.Visible)
            panel1.Visible = true;
        else
            panel1.Visible = false;
        ViewState["SEARCH"] = null;

        gvMulticolumn.EditIndex = -1;
        gvMulticolumn.SelectedIndex = -1;

        BindData();
        showbtn();
    }

    private void showbtn()
    {
        if (!panel1.Visible)
        {
            btnmulticolumn.ImageUrl = Session["images"] + "/arrowDown.png";
            btnmulticolumn.ToolTip = "Show All Port";
        }
        else
        {
            btnmulticolumn.ImageUrl = Session["images"] + "/te_del.png";
            btnmulticolumn.ToolTip = "Close";
        }
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
                panel1.Width = Unit.Parse(WidthVal);
                txtmuticolumn.Width = Unit.Parse((panel1.Width.Value - btnmulticolumn.Width.Value).ToString());
            }
        }
    }

    public string CssClass
    {
        set
        {
            txtmuticolumn.CssClass = value;
        }
    }

    public string SelectedValue
    {
        get
        {
            if (!string.IsNullOrEmpty(txtmuticolumn.Text))
                return txtmultiportvalue.Text.ToString();
            else
                return txtmuticolumn.Text;
        }
        set
        {
            txtmultiportvalue.Text = value;
        }
    }

    public string SelectedPortCallValue
    {
        get
        {
            if (!string.IsNullOrEmpty(txtmuticolumn.Text))
                return txtmultiportcallvalue.Text.ToString();
            else
                return txtmuticolumn.Text;
        }
        set
        {
            txtmultiportcallvalue.Text = value;
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

    public string VoyageId
    {
        get
        {
            return ViewState["VOYAGEID"] != null ? ViewState["VOYAGEID"].ToString() : "";
        }
        set
        {
            ViewState["VOYAGEID"] = value;
        }
    }

    public string VesselId
    {
        get
        {
            return ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : "";
        }
        set
        {
            ViewState["VESSELID"] = value;
        }
    }

    protected void btnsearch_click(object sender, EventArgs e)
    {
        ViewState["SEARCH"] = txtmuticolumn.Text;
        panel1.Visible = true;
        gvMulticolumn.EditIndex = -1;
        gvMulticolumn.SelectedIndex = -1;
        BindData();
    }

    private void BindData()
    {
        DataSet ds = PhoenixCrewOffshoreDMRVoyageData.ListVoyagePortDetails(
                     General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : "")
                    , General.GetNullableGuid(ViewState["VOYAGEID"] != null ? ViewState["VOYAGEID"].ToString() : ""));

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvMulticolumn.DataSource = ds;
            gvMulticolumn.DataBind();
        }
        else
        {
            gvMulticolumn.Visible = false;
        }

        showbtn();
    }

    protected void gvMulticolumn_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

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
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                panel1.Visible = false;
                txtmuticolumn.Text = ((LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkSeaportName")).Text.ToString();
                txtmultiportvalue.Text = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSeaportid")).Text.ToString();
                txtmultiportcallvalue.Text = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPortCallId")).Text.ToString();
                showbtn();
                OnTextChangedEvent(e);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
}
