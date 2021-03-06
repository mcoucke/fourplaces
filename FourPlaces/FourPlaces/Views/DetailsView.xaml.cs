﻿using FourPlaces.ViewModels;
using Storm.Mvvm.Forms;

namespace FourPlaces.Views
{
    public partial class DetailsView : BaseContentPage
    {
        public DetailsView(int id)
        {
            InitializeComponent();
            BindingContext = new DetailsViewModel(id);
        }
    }
}