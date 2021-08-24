using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using System.Data;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Drawing;
using Telerik.Web.UI;

public partial class PurchaseComments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta meta = new HtmlMeta();
        meta.HttpEquiv = "Refresh";
        Response.AppendHeader("Refresh", "300");

        if (!IsPostBack)
        {

            gvPhoenixNews.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Send", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
   
            MenuPhoenixBroadcast.MenuList = toolbar.Show();

            if (Request.QueryString["vesselid"] != null)
            {
                DataSet ds = PhoenixRegistersVessel.EditVesselCrewRelatedInfo(int.Parse(Request.QueryString["vesselid"].ToString()));

                if (ds.Tables.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    string supdt = dr["FLDSUPT"].ToString();
                    string purchaser = dr["FLDSUPPLIER"].ToString();

                    DataSet dsOffice = PhoenixRegistersVesselOfficeAdmin.OfficeAdminDetailsEdit(int.Parse(Request.QueryString["vesselid"].ToString()));

                    string purchasesupdt = "";

                    if (dsOffice.Tables.Count > 0 && dsOffice.Tables[0].Rows.Count > 0)
                    {
                        purchasesupdt = dsOffice.Tables[0].Rows[0]["FLDPURCHASESUPDT"].ToString();
                    }

                    if (PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString().Equals(supdt))
                    {
                        ViewState["SELECTEDUSER"] = purchaser;
                    }
                    else if (PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString().Equals(purchaser))
                    {
                        ViewState["SELECTEDUSER"] = supdt;
                    }
                    else if (PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString().Equals(purchasesupdt))
                    {
                        ViewState["SELECTEDUSER"] = supdt;
                    }
                }

                if (Request.QueryString["subject"] != null)
                {
                    ViewState["SUBJECT"] = Request.QueryString["subject"].ToString();
                    txtSubject.Text = ViewState["SUBJECT"].ToString();
                }
            }

            if (Request.QueryString["orderid"] != null)
            {
                ViewState["lblOrderId"] = Request.QueryString["orderid"].ToString();
            }
            else
                ViewState["lblOrderId"] = "";

            ViewState["PAGENUMBER"] = 1;           
        }
    }

    protected void PhoenixBroadcast_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (ViewState["SELECTEDUSER"] != null)
            {
                if (txtSubject.Text.Equals("") || txtMessage.Text.Equals(""))
                {
                    ucError.ErrorMessage = "Please enter both 'Subject' and 'Message'.";
                    ucError.Visible = true;
                    return;
                }
                PhoenixCommonBroadcast.Send(General.GetNullableInteger(ViewState["SELECTEDUSER"].ToString()), txtSubject.Text, txtMessage.Text);

                Reset();
                BindData();
                gvPhoenixNews.Rebind();

                BindUserListData();
            }
            else
            {
                ucError.ErrorMessage = "Please select the user from the left side to whom you want to send the message";
                ucError.Visible = true;
            }
        }
        if (CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
        }
    }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHEET"))
            {
                Response.Redirect("../Purchase/PurchaseStrategySheet.aspx?orderid=" + ViewState["lblOrderId"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindUserListData()
    {
        DataSet ds = PhoenixPurchaseBroadcast.ListUser(PhoenixSecurityContext.CurrentSecurityContext.UserCode);

        gvUsers.DataSource = ds;
     
        if (ds.Tables[0].Rows.Count > 0)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Purchase/PurchaseBroadcastUserAdd.aspx');", "Add User", "<i class=\"fas fa-search\"></i>", "");

            MenuUser.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Analysis", "SHEET", ToolBarDirection.Right);
            MenuOrderFormMain.Title = "Comments - (" + ds.Tables[0].Rows[0]["FLDTOTALUNREADMSGCOUNT"].ToString() + " Unread messages)";
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbar.Show();

            ViewState["SELECTEDUSER"] = ds.Tables[0].Rows[0]["FLDTOUSER"].ToString();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable ds = PhoenixPurchaseBroadcast.ReceiveMessage(
            int.Parse(ViewState["SELECTEDUSER"] == null ? "0" : ViewState["SELECTEDUSER"].ToString()),
            General.GetNullableString(txtSearch.Text),
            gvPhoenixNews.CurrentPageIndex + 1,
            gvPhoenixNews.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        gvPhoenixNews.DataSource = ds;
        gvPhoenixNews.VirtualItemCount = iRowCount;

        if (ds.Rows.Count > 0)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            LinkButton lblUserName = (LinkButton)gvUsers.Items[0].FindControl("lblUserName");

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Send", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);

            if (lblUserName != null)
            {
                MenuPhoenixBroadcast.Title = "To User: " + lblUserName.Text;
            }
            MenuPhoenixBroadcast.MenuList = toolbar.Show();
        }
  
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    private void Reset()
    {
        if (ViewState["SUBJECT"] != null)
        {
            txtSubject.Text = ViewState["SUBJECT"].ToString();
            txtMessage.Text = "";
        }
        else
        {
            txtMessage.Text = "";
            txtSubject.Text = "";
        }
    }
    
    protected void MenuUser_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("COMMENTS"))
            {
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindUserListData();
    }

    
    protected void cmdSearchAccount_Click(object sender, EventArgs e)
    {
        BindData();
        gvPhoenixNews.Rebind();
    }

    protected void gvUsers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindUserListData();
    }

    protected void gvUsers_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton lblUserName = (LinkButton)e.Item.FindControl("lblUserName");

            if (int.Parse(drv["FLDHISMSGCOUNT"].ToString().Equals("") ? "0" : drv["FLDHISMSGCOUNT"].ToString()) > 0 || int.Parse(drv["FLDMYMSGCOUNT"].ToString().Equals("") ? "0" : drv["FLDMYMSGCOUNT"].ToString()) > 0)
            {
                lblUserName.Font.Bold = true;
                lblUserName.ForeColor = Color.Blue;
            }
            else
            {
                lblUserName.Font.Bold = false;
                lblUserName.ForeColor = Color.Black;
            }


            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            Label lblId = (Label)e.Item.FindControl("lblId");

            if (lblId != null && lblId.Text != "")
            {
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }
            }
            else
            {
                if (db != null)
                {
                    db.Visible = false;
                }
            }
        }
    }

    protected void gvUsers_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SELECTUSER"))
        {
            Label lblToUserId = (Label)e.Item.FindControl("lblToUserId");
            LinkButton lblUserName = (LinkButton)e.Item.FindControl("lblUserName");

            ViewState["SELECTEDUSER"] = lblToUserId.Text;

            BindData();
            gvPhoenixNews.Rebind();


            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Send", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
            MenuPhoenixBroadcast.Title = "To User: " + lblUserName.Text;

            MenuPhoenixBroadcast.MenuList = toolbar.Show();
        }
    }

    protected void gvUsers_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        Label lblId = (Label)e.Item.FindControl("lblId");

        if (lblId != null && lblId.Text != "")
        {
            PhoenixPurchaseBroadcast.DeleteUser(new Guid(lblId.Text));
            BindUserListData();
            BindData();
            gvPhoenixNews.Rebind();
        }
    }

    protected void gvPhoenixNews_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvPhoenixNews_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
           
            LinkButton lblSub = (LinkButton)e.Item.FindControl("lblSub");

            if (lblSub != null)
            {
                if (drv["FLDREADYN"].ToString() == "0")
                {
                    lblSub.Font.Bold = true;
                    lblSub.ForeColor = Color.Blue;
                }
                else
                {
                    lblSub.Font.Bold = false;
                    lblSub.ForeColor = Color.Black;
                }
            }
        }
    }

    protected void gvPhoenixNews_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("REPLY"))
            {
                Label lblMessage = (Label)e.Item.FindControl("lblMessage");
                LinkButton lblSubject = (LinkButton)e.Item.FindControl("lblSubject");

                txtSubject.Text = lblSubject.Text;
                string message = @"


-------------
" + lblMessage.Text; ;

                txtMessage.Text = message;
                txtMessage.Focus();
            }

            if (e.CommandName.ToUpper().Equals("REPLYEDIT"))
            {
                TextBox lblMessage = (TextBox)e.Item.FindControl("lblMessageEdit");
                LinkButton lblSubjectedit = (LinkButton)e.Item.FindControl("lblSubjectEdit");

                txtSubject.Text = lblSubjectedit.Text;
                string message = @"


            -------------
            " + lblMessage.Text; ;

                txtMessage.Text = message;
                txtMessage.Focus();
            }

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindData();
                gvPhoenixNews.Rebind();

                Label lblId = (Label)e.Item.FindControl("lblId");
                Label lblRead = (Label)e.Item.FindControl("lblRead");
                Label lblToUser = (Label)e.Item.FindControl("lblToUser");

                if (lblRead.Text == "0" && lblToUser.Text == PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString())
                {
                    PhoenixCommonBroadcast.UpdateAsRead(new Guid(lblId.Text));

                    BindUserListData();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
