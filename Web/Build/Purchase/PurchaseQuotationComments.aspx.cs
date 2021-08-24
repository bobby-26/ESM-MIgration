using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;
public partial class PurchaseQuotationComments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "POSTCOMMENT", ToolBarDirection.Right);
        //MenuDiscussion.AccessRights = this.ViewState;
        MenuDiscussion.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() == "VENDOR")
            {
                if (PhoenixSecurityContext.CurrentSecurityContext == null)
                    PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;
            }

           
        }
    }

    protected void MenuDiscussion_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("POSTCOMMENT"))
        {
            if (!IsCommentValid(txtNotesDescription.Text))
            {
                ucError.Visible = true;
                return;
            }

            Guid Dtkey = Request.QueryString["Dtkey"] != null && General.GetNullableGuid(Request.QueryString["Dtkey"].ToString()) != null ? new Guid(Request.QueryString["Dtkey"].ToString()) : GetCurrentQuotationDTkey();

            PhoenixCommonDiscussion.TransTypeDiscussionInsert(
                Dtkey
                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , txtNotesDescription.Text.Trim(), "9");

            rgvComment.Rebind();
            txtNotesDescription.Text = "";
        }
    }

    private bool IsCommentValid(string strComment)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        return (!ucError.IsError);
    }

    private Guid GetCurrentQuotationDTkey()
    {
        Guid dtkey = Guid.Empty;

        try
        {
            DataSet ds = PhoenixPurchaseQuotation.EditQuotation(new Guid(Request.QueryString["QUOTATIONID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                dtkey = new Guid(ds.Tables[0].Rows[0]["FLDDTKEY"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        return dtkey;
    }

    protected void rgvComment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        Guid Dtkey = Request.QueryString["Dtkey"] != null && General.GetNullableGuid(Request.QueryString["Dtkey"].ToString()) != null ? new Guid(Request.QueryString["Dtkey"].ToString()) : GetCurrentQuotationDTkey();
        DataSet ds = new DataSet();
            ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(
            Dtkey,
            null, null, null
          , rgvComment.CurrentPageIndex+1, rgvComment.PageSize, ref iRowCount, ref iTotalPageCount, "9");

        rgvComment.DataSource = ds;
        rgvComment.VirtualItemCount = iRowCount;
        
        
    }
    protected void rgvComment_PreRender(object sender, EventArgs e)
    {

    }
}
