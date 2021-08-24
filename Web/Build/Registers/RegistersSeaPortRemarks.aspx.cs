using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class Registers_RegistersSeaPortRemarks : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        try
        {
            if (!IsPostBack)
            {
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

                if (Request.QueryString["seaportid"] != null)
                {
                    ViewState["seaportid"] = Request.QueryString["seaportid"];
                    BindDataEdit(ViewState["seaportid"].ToString());
                }

                MenuComment.AccessRights = this.ViewState;
                MenuComment.MenuList = toolbarmain.Show();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataEdit(string id)
    {
        DataSet ds = new DataSet();

        ds = PhoenixRegistersSeaport.EditSeaport(int.Parse(ViewState["seaportid"].ToString()), null);

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtProcedure.Content = ds.Tables[0].Rows[0]["FLDREMARKS"].ToString();
            //Title1.Text = "Remarks"+" (" + ds.Tables[0].Rows[0]["FLDSEAPORTNAME"].ToString() +")";
        }
    }

    protected void MenuComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixRegistersSeaport.UpdateSeaportRemarks(int.Parse(ViewState["seaportid"].ToString()), txtProcedure.Content);
            }

            BindDataEdit(ViewState["seaportid"].ToString());
            //ucStatus.Attributes["style"] = "top:0px:left:0px";
            //ucStatus.Text = "Remarks Updated";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
