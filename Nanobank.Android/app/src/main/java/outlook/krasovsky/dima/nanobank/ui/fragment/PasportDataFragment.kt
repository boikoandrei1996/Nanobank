package outlook.krasovsky.dima.nanobank.ui.fragment


import android.Manifest
import android.app.Activity
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
import android.graphics.Bitmap
import android.graphics.drawable.BitmapDrawable
import android.net.Uri
import android.os.Bundle
import android.provider.MediaStore
import android.support.v4.app.ActivityCompat
import android.support.v4.app.Fragment
import android.util.Base64
import android.util.Base64OutputStream
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import kotlinx.android.synthetic.main.fragment_pasport_data.*
import org.jetbrains.anko.backgroundDrawable
import org.jetbrains.anko.custom.async
import org.jetbrains.anko.support.v4.indeterminateProgressDialog
import org.jetbrains.anko.uiThread
import outlook.krasovsky.dima.nanobank.R
import outlook.krasovsky.dima.nanobank.ui.activity.SignInActivity.Companion.PERMISSION

import outlook.krasovsky.dima.nanobank.ui.activity.SignUpActivity
import outlook.krasovsky.dima.nanobank.ui.activity.intefrace.SignUpMoveInretface
import outlook.krasovsky.dima.nanobank.ui.fragment.`interface`.SignUpSaveParametor
import outlook.krasovsky.dima.nanobank.utils.model.SignUpModel
import java.io.ByteArrayOutputStream
import java.io.File
import java.net.URI
import java.net.URL


class PasportDataFragment : Fragment(),
        SignUpSaveParametor {

    private val TAKE_AVATAR_CAMERA_REQUEST = 1
    private val TAKE_AVATAR_GALLERY_REQUEST = 2

    private lateinit var listener: SignUpMoveInretface

    private var photo_pasport: Bitmap? = null

    override fun onCreateView(inflater: LayoutInflater?, container: ViewGroup?,
                              savedInstanceState: Bundle?): View? {
        return inflater!!.inflate(R.layout.fragment_pasport_data, container, false)
    }

    override fun onViewCreated(view: View?, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        if (photo_pasport == null) {
            listener.enableNext(false)
        } else {
            setImagePasport(photo_pasport!!)
        }
        initBtn()
        initData()
    }

    private fun initData() {
        listener.getDataCustomer().pasport = ""
    }

    private fun initBtn() {
        btn_pasport_camera.setOnClickListener { startCamera() }
        btn_pasport_galery.setOnClickListener { startGalery() }
        btn_pasport_galery.visibility = if (isPermissionGranted()) View.VISIBLE else View.GONE
    }

    private fun isPermissionGranted(): Boolean {
        val permissionCheck = ActivityCompat.checkSelfPermission(context, Manifest.permission.READ_EXTERNAL_STORAGE)
        return permissionCheck == PackageManager.PERMISSION_GRANTED
    }

    override fun onAttach(context: Context?) {
        super.onAttach(context)
        listener = context as SignUpActivity
    }

    private fun enableNext(isEnable: Boolean) {
        listener.enableNext(isEnable)
    }

    private fun startCamera() {
        val pictureIntent = Intent(
                MediaStore.ACTION_IMAGE_CAPTURE)
        startActivityForResult(pictureIntent, TAKE_AVATAR_CAMERA_REQUEST)
    }

    private fun startGalery() {
        val pickPhoto = Intent(Intent.ACTION_PICK);
        pickPhoto.type = "image/*";
        startActivityForResult(pickPhoto, TAKE_AVATAR_GALLERY_REQUEST);
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int,
                                  data: Intent?) {
        when (requestCode) {
            TAKE_AVATAR_CAMERA_REQUEST -> {
                if (resultCode == Activity.RESULT_OK && data != null) {
                    val cameraPic = data.getExtras().get("data") as Bitmap?
                    if (cameraPic != null) {
                        setImagePasport(cameraPic)
                    }
                }
            }
            TAKE_AVATAR_GALLERY_REQUEST -> {
                if (resultCode == Activity.RESULT_OK && data != null) {
                    val photoUri = data.data
                    val galleryPic = MediaStore.Images.Media.getBitmap(context.contentResolver, photoUri) as Bitmap?
                    if (galleryPic != null) {
                        setImagePasport(galleryPic)
                    }
                }
            }
        }
    }

    private fun setImagePasport(img: Bitmap) {
        enableNext(true)
        photo_pasport = img
        iv_pasport.backgroundDrawable = BitmapDrawable(resources, img)
    }

    private fun getByteArrayPasport(img: Bitmap): String {
        val stream = ByteArrayOutputStream()
        img.compress(Bitmap.CompressFormat.PNG, 11, stream)
        return Base64.encodeToString(stream.toByteArray(), Base64.DEFAULT)
    }

    override fun saveParam(data: SignUpModel) {
        val dialogWaiting = indeterminateProgressDialog("подождите, фото обрабатывается")
        dialogWaiting.setCancelable(false)
        dialogWaiting.show()
        async {
            data.pasport = getByteArrayPasport(photo_pasport!!)
            uiThread { dialogWaiting.dismiss() }
        }
    }

}
