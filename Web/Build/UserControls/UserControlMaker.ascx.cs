using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
 public partial class UserControlMaker : System.Web.UI.UserControl
 {
     private bool _appenddatabounditems;
     private int _selectedValue = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
             {
                 bind();
             }
         }

         public DataSet AddressList
         {
             set
             {
				 ddlAddress.DataBind();
				 ddlAddress.Items.Clear();
				 ddlAddress.DataSource = value;
				 ddlAddress.DataBind();
             }
         }
         public string AppendDataBoundItems
         {
             set
             {
                 if (value.ToUpper().Equals("TRUE"))
                     _appenddatabounditems = true;
                 else
                     _appenddatabounditems = false;
             }
         }

         public string CssClass
         {
             set
             {
                 ddlAddress.CssClass = value;
             }
         }


         public string AutoPostBack
         {
             set
             {
                 if (value.ToUpper().Equals("TRUE"))
                     ddlAddress.AutoPostBack = true;
             }
         }


         public string SelectedText
         {
             get
             {
				 return ddlAddress.SelectedItem.ToString();
             }
             set
             {
                 if (value == string.Empty)
                 {
                     ddlAddress.SelectedIndex = -1;
                     ddlAddress.ClearSelection();
                     ddlAddress.Text = string.Empty;
                     return;
                 }
                 _selectedValue = Int32.Parse(value);
                 ddlAddress.SelectedIndex = -1;
                 foreach (RadComboBoxItem item in ddlAddress.Items)
                 {
                     if (item.Value == _selectedValue.ToString())
                     {
                         item.Selected = true;
                         break;
                     }
                 }
             }
         }
      
         public string  SelectedValue
         {
             get
             {
				 return ddlAddress.SelectedValue;
             }
             set
             {
                 _selectedValue = Int32.Parse(value);
                 ddlAddress.SelectedIndex = -1;
                 foreach (RadComboBoxItem item in ddlAddress.Items)
                 {
                     if (item.Value == _selectedValue.ToString())
                     {
                         item.Selected = true;
                         break;
                     }
                 }
             }
         }
         private void bind()
         {
             ddlAddress.DataSource = PhoenixRegistersAddress.ListAddress("1");
             ddlAddress.DataBind();
         }
         protected void ddlAddress_DataBound(object sender, EventArgs e)
         {
             if (_appenddatabounditems)
                 ddlAddress.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
         } 
     }
 
 
