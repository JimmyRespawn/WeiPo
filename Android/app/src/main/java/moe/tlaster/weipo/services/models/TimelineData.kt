package moe.tlaster.weipo.services.models

import android.os.Parcelable
import kotlinx.android.parcel.Parcelize
import kotlinx.serialization.*
import kotlinx.serialization.internal.LongDescriptor
import kotlinx.serialization.internal.StringDescriptor
import kotlinx.serialization.json.JsonArray
import kotlinx.serialization.json.JsonInput
import kotlinx.serialization.json.JsonLiteral
import moe.tlaster.weipo.common.extensions.toObject

interface IWithUser {
    val user: User?
}

@Serializable
@Parcelize
data class TimelineData(
    val statuses: List<Status>? = null,

    val hasvisible: Boolean? = null,

    @SerialName("previous_cursor")
    val previousCursor: Long? = null,

    @SerialName("next_cursor")
    val nextCursor: Long? = null,

    @SerialName("previous_cursor_str")
    val previousCursorStr: String? = null,

    @SerialName("next_cursor_str")
    val nextCursorStr: String? = null,

    @SerialName("total_number")
    val totalNumber: Long? = null,

    val interval: Long? = null,

    @SerialName("uve_blank")
    val uveBlank: Long? = null,

    @SerialName("since_id")
    val sinceID: Long? = null,

    @SerialName("since_id_str")
    val sinceIDStr: String? = null,

    @SerialName("max_id")
    val maxID: Long? = null,

    @SerialName("max_id_str")
    val maxIDStr: String? = null,

    @SerialName("has_unread")
    val hasUnread: Long? = null
): Parcelable


@Serializable
sealed class Count : Parcelable {
    operator fun compareTo(i: Int): Int {
        return when (this) {
            is LongValue -> {
                value.compareTo(i)
            }
            is StringValue -> {
                Long.MAX_VALUE.compareTo(i)
            }
        }
    }

    override fun toString(): String {
        return when (this) {
            is LongValue -> value.toString()
            is StringValue -> value
        }
    }

    @Parcelize
    @Serializable
    class StringValue(val value: String) : Count()

    @Parcelize
    @Serializable
    class LongValue(val value: Long) : Count()

    @Serializer(forClass = Count::class)
    companion object : KSerializer<Count> {
        override val descriptor: SerialDescriptor =
            StringDescriptor.withName("Count")

        override fun serialize(encoder: Encoder, obj: Count) {
            when (obj) {
                is StringValue -> encoder.encode(StringValue.serializer(), obj)
                is LongValue -> encoder.encode(LongValue.serializer(), obj)
            }
        }

        override fun deserialize(decoder: Decoder): Count {
            return decoder.let {
                it as? JsonInput
            }?.decodeJson()?.let {
                when (it) {
                    is JsonLiteral -> {
                        if (it.isString) {
                            StringValue(it.toString())
                        } else {
                            LongValue(it.longOrNull ?: 0L)
                        }
                    }
                    else -> LongValue(0)
                }
            } ?: throw Error()
        }
    }
}

@Serializable
@Parcelize
data class Status(
    val visible: StatusVisible? = null,

    @SerialName("created_at")
    val createdAt: String? = null,

    override val id: String? = null,
    override val mid: String? = null,

    @SerialName("can_edit")
    val canEdit: Boolean? = null,

    @SerialName("show_additional_indication")
    val showAdditionalIndication: Long? = null,

    val text: String? = null,
    val textLength: Long? = null,
    val source: String? = null,
    val favorited: Boolean? = null,

    @SerialName("pic_ids")
    val picIDS: List<String> = listOf(),

    @SerialName("pic_types")
    val picTypes: String? = null,

    @SerialName("pic_focus_point")
    val picFocusPoint: List<PicFocusPoint>? = null,

    @SerialName("pic_flag")
    val picFlag: Long? = null,

    @SerialName("thumbnail_pic")
    val thumbnailPic: String? = null,

    @SerialName("bmiddle_pic")
    val bmiddlePic: String? = null,

    @SerialName("original_pic")
    val originalPic: String? = null,

    @SerialName("is_paid")
    val isPaid: Boolean? = null,

    @SerialName("mblog_vip_type")
    val mblogVipType: Long? = null,

    override val user: User? = null,

    @SerialName("reposts_count")
    val repostsCount: Count? = null,

    @SerialName("comments_count")
    val commentsCount: Count? = null,

    @SerialName("attitudes_count")
    val attitudesCount: Count? = null,

    @SerialName("pending_approval_count")
    val pendingApprovalCount: Long? = null,

    val isLongText: Boolean? = null,

    @SerialName("reward_exhibition_type")
    val rewardExhibitionType: Long? = null,

    @SerialName("hide_flag")
    val hideFlag: Long? = null,

    @SerialName("darwin_tags")
    val darwinTags: List<DarwinTag> = listOf(),

    val mblogtype: Long? = null,

    @SerialName("more_info_type")
    val moreInfoType: Long? = null,

    val cardid: String? = null,

    @SerialName("number_display_strategy")
    val numberDisplayStrategy: NumberDisplayStrategy? = null,

    @SerialName("content_auth")
    val contentAuth: Long? = null,

    @SerialName("pic_num")
    val picNum: Long? = null,

    val pics: List<Pic>? = null,
    val bid: String? = null,
    val pid: Long? = null,
    val pidstr: String? = null,

    @SerialName("retweeted_status")
    val retweetedStatus: Status? = null,

    @SerialName("raw_text")
    val rawText: String? = null,

    @SerialName("reward_scheme")
    val rewardScheme: String? = null,

    val title: Title? = null,

    @SerialName("page_info")
    val pageInfo: PageInfo? = null,

    @SerialName("safe_tags")
    val safeTags: Long? = null
): Parcelable, IWithUser, ICanReply

@Serializable
@Parcelize
data class DarwinTag(
    @SerialName("object_type")
    val objectType: String? = null,

    @SerialName("object_id")
    val objectID: String? = null,

    @SerialName("display_name")
    val displayName: String? = null,

    @SerialName("bd_object_type")
    val bdObjectType: String? = null
): Parcelable

@Serializable
@Parcelize
data class NumberDisplayStrategy(
    @SerialName("apply_scenario_flag")
    val applyScenarioFlag: Long? = null,

    @SerialName("display_text_min_number")
    val displayTextMinNumber: Long? = null,

    @SerialName("display_text")
    val displayText: String? = null
): Parcelable

@Serializable
@Parcelize
data class PageInfo(
    @SerialName("object_type")
    val objectType: Long? = null,

    val type: String? = null,

    @SerialName("page_pic")
    val pagePic: PagePic? = null,

    @SerialName("page_url")
    val pageURL: String? = null,

    @SerialName("page_title")
    val pageTitle: String? = null,

    val content1: String? = null,

    @SerialName("object_id")
    val objectID: String? = null,

    val title: String? = null,
    val content2: String? = null,

    @SerialName("video_orientation")
    val videoOrientation: String? = null,

    @SerialName("play_count")
    val playCount: String? = null,

    @SerialName("media_info")
    val mediaInfo: MediaInfo? = null,

    val urls: Map<String, String>? = null,

    @SerialName("video_details")
    val videoDetails: VideoDetails? = null
): Parcelable

@Serializable
@Parcelize
data class PagePic(
    @Serializable(with = CustomLongSerializer::class)
    val width: Long? = null,
    val url: String? = null,
    @Serializable(with = CustomLongSerializer::class)
    val height: Long? = null
): Parcelable

@Serializable
@Parcelize
data class PicFocusPoint(
    @SerialName("focus_point")
    val focusPoint: FocusPoint? = null,

    @SerialName("pic_id")
    val picID: String? = null
): Parcelable

@Serializable
@Parcelize
data class FocusPoint(
    val left: Double? = null,
    val top: Double? = null,
    val width: Double? = null,
    val height: Double? = null,
    val type: Long? = null
): Parcelable

@Serializable
@Parcelize
data class Pic(
    val pid: String? = null,
    val url: String? = null,
    val size: String? = null,
    val geo: PicGeo? = null,
    val large: PurpleLarge? = null
): Parcelable

@Serializer(forClass = Long::class)
object CustomLongSerializer : KSerializer<Long> {
    override val descriptor: SerialDescriptor
        get() = LongDescriptor

    override fun serialize(encoder: Encoder, obj: Long) {
        encoder.encodeLong(obj)
    }

    override fun deserialize(decoder: Decoder): Long {
        return decoder.decodeString().toLongOrNull() ?: 0L
    }
}

@Serializable
@Parcelize
data class PicGeo(
    @Serializable(with = CustomLongSerializer::class)
    val width: Long? = null,
    @Serializable(with = CustomLongSerializer::class)
    val height: Long? = null,
    val croped: Boolean? = null
): Parcelable

@Serializable
@Parcelize
data class PurpleLarge(
    val size: String? = null,
    val url: String? = null,
    val geo: PurpleGeo? = null
): Parcelable

@Serializable
@Parcelize
data class PurpleGeo(
    @Serializable(with = CustomLongSerializer::class)
    val width: Long? = null,
    @Serializable(with = CustomLongSerializer::class)
    val height: Long? = null,
    val croped: Boolean? = null
): Parcelable

@Serializable
@Parcelize
data class MediaInfo(
    @SerialName("stream_url")
    val streamURL: String? = null,

    @SerialName("mp4_720p_mp4")
    val mp4_720p_mp4: String? = null,

    @SerialName("stream_url_hd")
    val streamURLHD: String? = null,

    val duration: Double = 0.0
): Parcelable

@Serializable
@Parcelize
data class VideoDetails(
    val size: Long? = null,
    val bitrate: Long? = null,
    val label: String? = null,

    @SerialName("prefetch_size")
    val prefetchSize: Long = 0
): Parcelable

@Serializable
@Parcelize
data class User(
    @SerialName("avatar_large")
    val avatarLarge: String? = null,

    val id: Long? = null,

    @SerialName("screen_name")
    val screenName: String? = null,

    @SerialName("profile_image_url")
    val profileImageURL: String? = null,

    @SerialName("profile_url")
    val profileURL: String? = null,

    @SerialName("statuses_count")
    val statusesCount: Long? = null,

    val verified: Boolean? = null,

    @SerialName("verified_type")
    val verifiedType: Long? = null,

    @SerialName("verified_type_ext")
    val verifiedTypeEXT: Long? = null,

    @SerialName("verified_reason")
    val verifiedReason: String? = null,

    @SerialName("close_blue_v")
    val closeBlueV: Boolean? = null,

    val description: String? = null,
    val gender: String? = null,
    val mbtype: Long? = null,
    val urank: Long? = null,
    val mbrank: Long? = null,

    @SerialName("follow_me")
    val followMe: Boolean? = null,

    val following: Boolean? = null,

    @SerialName("followers_count")
    val followersCount: Long? = null,

    @SerialName("follow_count")
    val followCount: Long? = null,

    @SerialName("cover_image_phone")
    val coverImagePhone: String? = null,

    @SerialName("avatar_hd")
    val avatarHD: String? = null,

    val like: Boolean? = null,

    @SerialName("like_me")
    val likeMe: Boolean? = null
): Parcelable

@Serializable
@Parcelize
data class Title(
    val text: String? = null
): Parcelable

@Serializable
@Parcelize
data class StatusVisible(
    val type: Long? = null,

    @SerialName("list_id")
    val listID: Long? = null,

    @SerialName("list_idstr")
    val listIdstr: String? = null
): Parcelable


@Serializable
sealed class Comments : Parcelable {
    @Parcelize
    @Serializable
    data class BoolValue(val value: Boolean) : Comments()

    @Parcelize
    @Serializable
    data class ListValue(val value: List<Comment>) : Comments()

    @Serializer(forClass = Comments::class)
    companion object : KSerializer<Comments> {
        override val descriptor: SerialDescriptor =
            StringDescriptor.withName("Comments")

        override fun serialize(encoder: Encoder, obj: Comments) {
            when (obj) {
                is BoolValue -> encoder.encode(BoolValue.serializer(), obj)
                is ListValue -> encoder.encode(ListValue.serializer(), obj)
            }
        }

        @UseExperimental(ImplicitReflectionSerializer::class)
        override fun deserialize(decoder: Decoder): Comments {
            return decoder.let {
                it as? JsonInput
            }?.decodeJson()?.let {

                when (it) {
                    is JsonArray -> {
                        ListValue(it.map { it.jsonObject.toObject<Comment>() })
                    }
                    is JsonLiteral -> {
                        BoolValue(it.booleanOrNull ?: false)
                    }
                    else -> {
                        BoolValue(false)
                    }
                }
            } ?: throw Error()
        }
    }
}

@Serializable
@Parcelize
data class Comment(

    @SerialName("pic")
    val pic: Pic? = null,

    @SerialName("comment_badge")
    val commentBadge: List<CommentBadge>? = null,

    val readtimetype: String? = null,

    val comments: Comments? = null,

    @SerialName("max_id")
    val maxID: Long? = null,

    @SerialName("total_number")
    val totalNumber: Long? = null,

    val isLikedByMblogAuthor: Boolean? = null,

    @SerialName("more_info_type")
    val moreInfoType: Long? = null,


    @SerialName("like_count")
    val likeCount: Long? = null,

    val rootid: String? = null,

    @SerialName("disable_reply")
    val disableReply: Long? = null,

    @SerialName("created_at")
    val createdAt: String? = null,

    override val mid: String? = null,

    @SerialName("floor_number")
    val floorNumber: Long? = null,

    val source: String? = null,
    val rootidstr: String? = null,

    @SerialName("reply_count")
    val replyCount: Long? = null,

    val liked: Boolean? = null,
    override val id: String? = null,
    val text: String? = null,
    override val user: User? = null,
    val status: Status? = null,
    val bid: String? = null
): Parcelable, IWithUser, ICanReply

@Serializable
@Parcelize
data class CommentBadge(
    @SerialName("pic_url")
    val picURL: String? = null,

    val name: String? = null,
    val length: Double? = null,
    val actionlog: Actionlog? = null,
    val scheme: String? = null
): Parcelable

@Serializable
@Parcelize
data class Actionlog(
    @SerialName("act_code")
    val actCode: String? = null,

    val ext: String? = null
): Parcelable

@Serializable
@Parcelize
data class Attitude(
    val idStr: String? = null,

    @SerialName("attitude_type")
    val attitudeType: Long? = null,

    @SerialName("source_allowclick")
    val sourceAllowclick: Long? = null,

    @SerialName("attitude_mask")
    val attitudeMask: Long? = null,

    @SerialName("last_attitude")
    val lastAttitude: String? = null,

    @SerialName("created_at")
    val createdAt: String? = null,

    @SerialName("source_type")
    val sourceType: Long? = null,

    val id: Long? = null,
    val source: String? = null,
    override val user: User? = null,
    val attitude: String? = null,
    val status: Status? = null
): Parcelable, IWithUser

@Serializable
@Parcelize
data class MessageList(
    val user: User? = null,

    @SerialName("created_at")
    val createdAt: String? = null,

    val scheme: String? = null,
    val unread: Long? = null,
    val text: String? = null
): Parcelable

