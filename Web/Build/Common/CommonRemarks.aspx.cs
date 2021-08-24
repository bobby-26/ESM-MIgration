using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
public partial class CommonRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
            ViewState["CURRENTINDEX"] = 1;

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Post Remarks", "POSTCOMMENT", ToolBarDirection.Right);
            Remarks.AccessRights = this.ViewState;
            Remarks.MenuList = toolbar.Show();

            ViewState["DTKEY"] = null;
            ViewState["Applncode"] = null;
            ViewState["flag"] = null;
            if (Request.QueryString["dtkey"] != null && Request.QueryString["dtkey"].ToString() != string.Empty)
                ViewState["DTKEY"] = Request.QueryString["dtkey"].ToString();
            if (Request.QueryString["Applncode"] != null && Request.QueryString["Applncode"].ToString() != string.Empty)
                ViewState["Applncode"] = Request.QueryString["Applncode"].ToString();


            BindDataNote();

        }
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
    }


    protected void Remarks_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("POSTCOMMENT"))
            {
                if (!IsCommentValid(txtNotesDescription.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCommonDiscussion.TransTypeDiscussionInsert(new Guid(ViewState["DTKEY"].ToString())
                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , txtNotesDescription.Text.Trim(), ViewState["Applncode"].ToString());

                BindDataNote();

                txtNotesDescription.Text = "";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindDataNote()
    {
        DataSet ds;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ucUser.SelectedUser != "")
        {
            ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(General.GetNullableGuid(ViewState["DTKEY"] == null ? null : ViewState["DTKEY"].ToString()), null, sortexpression, sortdirection
              , (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, ViewState["Applncode"].ToString(), General.GetNullableInteger(ucUser.SelectedUser));
        }
        else
        {
            ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(General.GetNullableGuid(ViewState["DTKEY"] == null ? null : ViewState["DTKEY"].ToString()), null, sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, ViewState["Applncode"].ToString(), null);

        }


        if (ds.Tables[0].Rows.Count > 0)
        {
            repDiscussion.DataSource = ds.Tables[0];
            repDiscussion.DataBind();
        }
        else
        {
            ShowNoRecordsFound(ds.Tables[0], repDiscussion);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    private void ShowNoRecordsFound(DataTable dt, Repeater rep)
    {
        dt.Rows.Add(dt.NewRow());
        rep.DataSource = dt;
        rep.DataBind();
    }

    protected void repDiscussion_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }

    protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        BindDataNote();
    }

    private bool IsCommentValid(string strComment)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        return (!ucError.IsError);
    }
}
