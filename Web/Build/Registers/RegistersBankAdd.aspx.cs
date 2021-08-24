using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Registers_RegistersBankAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain = new PhoenixToolbar();
                ViewState["ID"] = Request.QueryString["id"];
                if (ViewState["ID"] != null && ViewState["ID"].ToString() != "")
                    Edit(new Guid(ViewState["ID"].ToString()));

                toolbarmain = new PhoenixToolbar();
                MenuBank1.Title = "Bank Details";
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbarmain.AddButton("List", "LIST", ToolBarDirection.Right);
                MenuBank1.AccessRights = this.ViewState;
                MenuBank1.MenuList = toolbarmain.Show();
            }

            if(Request.QueryString["PageNo"] != null)
            {
                ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["PageNo"].ToString());
            }
            else
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Edit(Guid Dtkey)
    {
        DataSet ds = PhoenixRegistersBank.EditBank(Dtkey);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtname.Text = ds.Tables[0].Rows[0]["FLDNAME"].ToString();
            txtShortcode.Text = ds.Tables[0].Rows[0]["FLDSHORTCODE"].ToString();
            txtacnopattern.Text = ds.Tables[0].Rows[0]["FLDACCOUNTNOPATTERN"].ToString();
            txtacnoDigits.Text = ds.Tables[0].Rows[0]["FLDACCOUNTNODIGIT"].ToString();
            if (ds.Tables[0].Rows[0]["FLDISACTIVE"].ToString() == "1")
                chkActiveYN.Checked = true;
            if (ds.Tables[0].Rows[0]["FLDALPHANUMERICYN"].ToString() == "1")
                chkAllowCharacterYN.Checked = true;
            txtswiftcodedigits.Text = ds.Tables[0].Rows[0]["FLDSWIFTCODENODIGIT"].ToString();
        }
    }
    protected void MenuBank1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Registers/RegistersBank.aspx?PageNo=" + ViewState["PAGENUMBER"].ToString(), false);
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidBank())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["ID"] != null && ViewState["ID"].ToString() != "")
                {
                    PhoenixRegistersBank.UpdateBank(
                      new Guid(ViewState["ID"].ToString())
                       , txtname.Text.Trim()
                       , txtShortcode.Text.Trim()
                       , txtacnopattern.Text.Trim()
                       , null
                       , chkActiveYN.Checked.Equals(true) ? 1 : 0
                       , chkAllowCharacterYN.Checked.Equals(true) ? 1 : 0
                       , int.Parse(txtacnoDigits.Text.Trim())
                       , int.Parse(txtswiftcodedigits.Text.Trim())
                    );
                    ucStatus.Text = "Bank Details updated successfully.";
                }
                else
                {
                    PhoenixRegistersBank.InsertBank(txtname.Text.Trim()
                        , txtShortcode.Text.Trim()
                        , txtacnopattern.Text.Trim()
                        , null
                        , chkActiveYN.Checked.Equals(true) ? 1 : 0
                        , chkAllowCharacterYN.Checked.Equals(true) ? 1 : 0
                        , int.Parse(txtacnoDigits.Text.Trim())
                        , int.Parse(txtswiftcodedigits.Text.Trim())
                    );
                    ucStatus.Text = "Bank Details added successfully.";
                }
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidBank()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (txtname.Text.Trim() == "")
        {
            ucError.ErrorMessage = "Name is required.";
        }
        if (txtShortcode.Text.Trim() == "")
        {
            ucError.ErrorMessage = "Short name is required.";
        }
        if (txtacnoDigits.Text.Trim() == "")
        {
            ucError.ErrorMessage = "A/C No Digits is  required.";
        }
        else if (int.Parse(txtacnoDigits.Text.Trim()) <= 0)
        {
            ucError.ErrorMessage = "A/C No Digits is greater than Zero.";
        }
        if (txtswiftcodedigits.Text.Trim() == "")
        {
            ucError.ErrorMessage = "Swift Code Digits is  required.";
        }
        else if (int.Parse(txtswiftcodedigits.Text.Trim()) <= 0)
        {
            ucError.ErrorMessage = "Swift Code Digits is greater than Zero.";
        }
        //if (txtacnopattern.Text.Trim() == "")
        //{
        //    ucError.ErrorMessage = "A/C No Format is  required.";
        //}
        return (!ucError.IsError);
    }
}