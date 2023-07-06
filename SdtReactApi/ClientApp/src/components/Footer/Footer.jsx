import React from 'react'
import './Footer.css'
import { GrFacebookOption,GrLinkedinOption } from 'react-icons/gr';
import { AiOutlineTwitter } from 'react-icons/ai';


const Footer = () => {
  return (
    <div className='Footer-body'>
      <div className='Footer'>
        <div className='Social-media'>
         <a href="https://www.facebook.com/people/Smartdust-Technologies/100071813210648/" target='_blank'><GrFacebookOption/></a> 
         <a href="" target='_blank'><AiOutlineTwitter/></a> 
         <a href="" target='_blank'><GrLinkedinOption/></a> 
        </div>
        <div className='Footer-text'>
          <p>©2021 by Smartdust Technologies Pvt. Ltd.</p>
        </div>
      </div>
    </div>
  )
}

export default Footer
