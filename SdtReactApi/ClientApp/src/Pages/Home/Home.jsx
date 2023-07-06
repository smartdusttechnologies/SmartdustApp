import React from 'react'
import './Home.css'
import SlickGoTo from '../../components/AutoSlider/AutoSlider'
import Contact from '../Contact/Contact'
import Image_Slider_2 from '../../assets/images/Image_Slider_2.webp'

const Home = () => {
  return (
    <div className='Home-body'>
      <div className='Poster-Home'>
       <div className='Poster-Home-text'> <p>INTRODUCING <br /> SMARTDUST <br /> TECHNOLOGIES <br /> <span>Practical Solutions</span> </p></div>
      </div>
      
      {/* About Section  */}
      <div className='About'>
        <div className='About-container'>
          
          <p id='About-Q'>WHO WE ARE</p>
          <p id='About-text'>We’re a young and talented group of entrepreneurs and engineers with a groundbreaking idea that we hope will contribute towards a better tomorrow. We provide smart solutions for companies of all sizes and pride ourselves on our unparalleled, dedicated service.</p>
          <p id='About-text'>At Smartdust Technologies, we believe that the right understanding and technological edge can lead companies towards a successful future. We always seek valuable feedback from our clients in order to learn and evolve. Contact us today to set up a meeting with one of our sales representatives or request a demo.</p>
        </div>
      </div>

      {/* Auto Slider  */}
      <div className='Auto-slider'>
        <SlickGoTo  />
      </div>

      {/* About Section  */}
      <div className='work About'>
        <div className='About-container'>
          
          <p id='About-Q'>WHAT WE DO</p>
          <p id='About-text'>At Smartdust Technologies, we believe that our solutions will soon become one of the biggest segments in the industry. We’ve only just started, but we already know that every product we build requires hard-earned skills, dedication and a daring attitude. Continue reading and learn all there is to know about the smart tech behind our successful Information Technology Startup Company.</p>
        </div>
      </div>

      {/* Auto Slider  */}
      <div className='Auto-slider'>
        <SlickGoTo/>
      </div>

      <div className='Quote-div none'>
        <div>
        <p id='quote'>"Well done is better than well said"</p>
        <p>Benjamin Franklin</p>
        </div>
      </div>

      {/* Quote For Small screen  */}
      <div className='small-screen-quote'>
        <div>
        <img src={Image_Slider_2} alt="" />
        </div>
        <div className='s-quote-div'>
          <div>
            <p id='quote'>"Well done is better than well said"</p>
            <p>Benjamin Franklin</p>
          </div>
        </div>
      </div>
      
      {/* Contact  */}
      <Contact />
    </div>
  )
}

export default Home

