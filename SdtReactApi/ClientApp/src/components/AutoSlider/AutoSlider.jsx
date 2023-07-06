import React, { Component } from "react";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css"; 
import "slick-carousel/slick/slick-theme.css";
import './AutoSlider.css'
import Image_Slider_1 from '../../assets/images/Image_Slider_1.webp'
import Image_Slider_2 from '../../assets/images/Image_Slider_2.webp'
import Image_Slider_3 from '../../assets/images/Image_Slider_3.webp'
import Image_Slider_4 from '../../assets/images/Image_Slider_4.webp'
import Image_Slider_5 from '../../assets/images/Image_Slider_5.webp'
import Image_Slider_6 from '../../assets/images/Image_Slider_6.webp'


export default class SlickGoTo extends React.Component {
  state = {
    slideIndex: 0,
    updateCount: 0
  };

  render() {
    const settings = {
      dots: false,
      infinite: true,
      speed: 500,
      slidesToShow: 1,
      slidesToScroll: 1,
      autoplay: true,
      autoplaySpeed: 2000,

      afterChange: () =>
      this.setState(state => ({ updateCount: state.updateCount + 1 })),
    beforeChange: (current, next) => this.setState({ slideIndex: next })
    };

    const images = [ Image_Slider_1 , Image_Slider_2,Image_Slider_3,Image_Slider_4,Image_Slider_5,Image_Slider_6]

    return (
      <div style={{width:'100%'}}>
        <Slider ref={slider => (this.slider = slider)} {...settings} arrows={false}>
            {
                images.map((i)=>(
                    <div className="image-slider-body" key={i}>
                        <img src={i} alt="" />
                    </div>
                ))
            }
        </Slider>
      </div>
    );
  }
}