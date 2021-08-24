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
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Common;


public partial class InventoryComponentTypeDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE");
            MenuComponentTypeDetail.AccessRights = this.ViewState;   
            MenuComponentTypeDetail.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                BindFields();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFields()
    {
        try
        {
            if ((Request.QueryString["COMPONENTTYPEID"] != null) && (Request.QueryString["COMPONENTTYPEID"] != ""))
            {
                string str = Request.QueryString["COMPONENTTYPEID"].ToString();
                DataSet ds = PhoenixInventoryComponentType.ListComponentType(new Guid(Request.QueryString["COMPONENTTYPEID"].ToString()));
                DataRow dr = ds.Tables[0].Rows[0];
                txtComponentTypeDetail.Content= dr["FLDNOTES"].ToString();
                ViewState["dtkey"] = dr["FLDDTKEY"].ToString();

                Title1.Text += "    (Component Type - " + dr["FLDCOMPONENTNUMBER"].ToString() + " )";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuComponentTypeDetail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {

                if ((Request.QueryString["COMPONENTTYPEID"] != null) && (Request.QueryString["COMPONENTTYPEID"] != ""))
                {

                    PhoenixInventoryComponentType.UpdateDetailsComponentType(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(Request.QueryString["COMPONENTTYPEID"].ToString())
                        , txtComponentTypeDetail.Content.ToString());

                    ucStatus.Text = "Details Saved.";
                }
            }
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void btnInsertPic_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.Files.Count > 0 && txtComponentTypeDetail.ActiveMode == AjaxControlToolkit.HTMLEditor.ActiveModeType.Design)
            {
                Guid gFileName = PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.INVENTORY, null, ".jpg,.png,.gif");
                DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["dtkey"].ToString()));
                DataRow[] dr = dt.Select("FLDDTKEY = '" + gFileName.ToString() + "'");
                if (dr.Length > 0)
                    txtComponentTypeDetail.Content = txtComponentTypeDetail.Content + "<img src=\"" + HttpContext.Current.Session["sitepath"] + "/attachments/" + dr[0]["FLDFILEPATH"].ToString() + "\" />";
            }
            else
            {
                ucError.Text = Request.Files.Count > 0 ? "You are not in design mode" : "No Picture selected.";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
